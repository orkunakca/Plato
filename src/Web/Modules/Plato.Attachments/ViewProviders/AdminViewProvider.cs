﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using PlatoCore.Models.Shell;
using Plato.Attachments.Models;
using Plato.Attachments.Stores;
using Plato.Attachments.ViewModels;
using PlatoCore.Hosting.Abstractions;
using Plato.Roles.ViewModels;
using PlatoCore.Stores.Roles;
using PlatoCore.Models.Roles;
using PlatoCore.Data.Abstractions;
using Plato.Attachments.Extensions;
using PlatoCore.Navigation.Abstractions;
using PlatoCore.Stores.Abstractions.Roles;
using Microsoft.AspNetCore.Mvc.Rendering;
using PlatoCore.Abstractions.Extensions;
using PlatoCore.Layout.ViewProviders.Abstractions;

namespace Plato.Attachments.ViewProviders
{
    public class AdminViewProvider : ViewProviderBase<AttachmentSetting>
    {

        public static readonly long[] SizesInBytes = new long[] {
            51200,
            102400,
            524288,
            1048576,
            2097152,
            3145728,
            4194304,
            5242880,
            10485760,
            20971520,
            31457280,
            41943040,
            52428800,
            62914560,
            73400320,
            83886080,
            94371840,
            104857600,
            209715200,
            314572800,
            419430400,
            524288000,
            1073741824,
            2147483648,
            4294967296,
            8589934592,
            17179869184,
            34359738368,
            68719476736,
            137438953472,
            274877906944
        };

        private readonly IAttachmentSettingsStore<AttachmentSettings> _attachmentSettingsStore;        
        private readonly ILogger<AdminViewProvider> _logger;
        private readonly IPlatoRoleStore _platoRoleStore;
        private readonly IShellSettings _shellSettings;
        private readonly IPlatoHost _platoHost;

        private readonly HttpRequest _request;

        public const string ExtensionHtmlName = "extension";

        public AdminViewProvider(
            IAttachmentSettingsStore<AttachmentSettings> attachmentSettingsStore,
            IHttpContextAccessor httpContextAccessor,     
            IPlatoRoleStore platoRoleStore,
            ILogger<AdminViewProvider> logger,
            IShellSettings shellSettings,     
            IPlatoHost platoHost)
        {
            _request = httpContextAccessor.HttpContext.Request;
            _attachmentSettingsStore = attachmentSettingsStore;            
            _platoRoleStore = platoRoleStore;
            _shellSettings = shellSettings;
            _platoHost = platoHost;
            _logger = logger;
        }

        public override async Task<IViewProviderResult> BuildIndexAsync(AttachmentSetting settings, IViewProviderContext context)
        {

            var viewModel = context.Controller.HttpContext.Items[typeof(AttachmentsIndexViewModel)] as AttachmentsIndexViewModel;
            if (viewModel == null)
            {
                throw new Exception($"A view model of type {typeof(AttachmentsIndexViewModel).ToString()} has not been registered on the HttpContext!");
            }

            viewModel.Results = await GetRoles(viewModel.Options, viewModel.Pager);

            return Views(
                View<AttachmentsIndexViewModel>("Admin.Index.Header", model => viewModel).Zone("header").Order(1),
                View<AttachmentsIndexViewModel>("Admin.Index.Tools", model => viewModel).Zone("tools").Order(1),
                View<AttachmentsIndexViewModel>("Admin.Index.Content", model => viewModel).Zone("content").Order(1)
            );

        }

        public override Task<IViewProviderResult> BuildDisplayAsync(AttachmentSetting settings, IViewProviderContext context)
        {
            return Task.FromResult(default(IViewProviderResult));
        }

        public override async Task<IViewProviderResult> BuildEditAsync(AttachmentSetting setting, IViewProviderContext context)
        {

            var role = await _platoRoleStore.GetByIdAsync(setting.RoleId);

            if (role == null)
            {
                return default(IViewProviderResult);
            }

            // Defaults
            long availableSpace = 0;
            var allowedExtensions = DefaultExtensions.AllowedExtensions;

            // Populate settings
            var settings = await _attachmentSettingsStore.GetByRoleIdAsync(role.Id);
            if (settings != null)
            {
                availableSpace = settings.AvailableSpace;
                allowedExtensions = settings.AllowedExtensions;
            }

            // Build model
            var viewModel = new EditAttachmentSettingsViewModel()
            {
                RoleId = role.Id,
                Role = role,
                AvailableSpace = availableSpace,
                AvailableSpaces = GetAvailableSpaces(),
                DefaultExtensions = DefaultExtensions.Extensions,
                ExtensionHtmlName = ExtensionHtmlName,
                AllowedExtensions = allowedExtensions
            };

            // Build view
            return Views(
                View<EditAttachmentSettingsViewModel>("Admin.Edit.Header", model => viewModel).Zone("header").Order(1),
                View<EditAttachmentSettingsViewModel>("Admin.Edit.Tools", model => viewModel).Zone("tools").Order(1),
                View<EditAttachmentSettingsViewModel>("Admin.Edit.Content", model => viewModel).Zone("content").Order(1)
            );

        }

        public override async Task<IViewProviderResult> BuildUpdateAsync(AttachmentSetting settings, IViewProviderContext context)
        {

            var model = new EditAttachmentSettingsViewModel();

            // Validate model
            if (!await context.Updater.TryUpdateModelAsync(model))
            {
                return await BuildEditAsync(settings, context);
            }

            // Update settings
            if (context.Updater.ModelState.IsValid)
            {

                settings = new AttachmentSetting()
                {
                    RoleId = model.RoleId,
                    AvailableSpace = model.AvailableSpace,
                    AllowedExtensions = GetPostedExtensions()
                };

                var result = await _attachmentSettingsStore.SaveAsync(settings);
                if (result != null)
                {
                    // Recycle shell context to ensure changes take effect
                    _platoHost.RecycleShellContext(_shellSettings);
                }

            }

            return await BuildEditAsync(settings, context);

        }

        // -----------------------

        async Task<IPagedResults<Role>> GetRoles(
            RoleIndexOptions options,
            PagerOptions pager)
        {
            return await _platoRoleStore.QueryAsync()
                .Take(pager.Page, pager.Size, pager.CountTotal)
                .Select<RoleQueryParams>(q =>
                {
                    if (options.RoleId > 0)
                    {
                        q.Id.Equals(options.RoleId);
                    }
                    if (!string.IsNullOrEmpty(options.Search))
                    {
                        q.Keywords.Like(options.Search);
                    }
                })
                .OrderBy("ModifiedDate", OrderBy.Desc)
                .ToList();
        }

        string[] GetPostedExtensions()
        {

            if (!_request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            var extensions = new List<string>();
            foreach (var key in _request.Form?.Keys)
            {
                if (key == ExtensionHtmlName)
                {
                    var values = _request.Form[key];
                    foreach (var value in values)
                    {
                        if (!String.IsNullOrEmpty(value))
                        {
                            if (!extensions.Contains(value))                            
                                extensions.Add(value);
                        }
                    }
                }
            }

            return extensions.ToArray();

        }

        IEnumerable<SelectListItem> GetAvailableSpaces()
        {

            var output = new List<SelectListItem>();
            foreach (var size in SizesInBytes)
            {
                output.Add(new SelectListItem
                {
                    Text = size.ToFriendlyFileSize(),
                    Value = size.ToString()
                });
            }

            return output;

        }

    }

}