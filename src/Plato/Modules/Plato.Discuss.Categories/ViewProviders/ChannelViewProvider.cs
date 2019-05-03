﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Plato.Categories.Models;
using Plato.Categories.Services;
using Plato.Categories.Stores;
using Plato.Categories.ViewModels;
using Plato.Discuss.Categories.Models;
using Plato.Discuss.Categories.ViewModels;
using Plato.Discuss.Models;
using Plato.Entities.ViewModels;
using Plato.Internal.Features.Abstractions;
using Plato.Internal.Hosting.Abstractions;
using Plato.Internal.Layout.ViewProviders;

namespace Plato.Discuss.Categories.ViewProviders
{
    public class ChannelViewProvider : BaseViewProvider<Channel>
    {

        private readonly IContextFacade _contextFacade;
        private readonly ICategoryStore<Channel> _categoryStore;
        private readonly ICategoryManager<Channel> _categoryManager;
        private readonly IFeatureFacade _featureFacade;
        private readonly IActionContextAccessor _actionContextAccessor;

        public ChannelViewProvider(
            IContextFacade contextFacade,
            ICategoryStore<Channel> categoryStore,
            ICategoryManager<Channel> categoryManager,
            IFeatureFacade featureFacade,
            IActionContextAccessor actionContextAccessor)
        {
            _contextFacade = contextFacade;
            _categoryStore = categoryStore;
            _categoryManager = categoryManager;
            _featureFacade = featureFacade;
            _actionContextAccessor = actionContextAccessor;
        }

        public override async Task<IViewProviderResult> BuildIndexAsync(Channel channelAdmin, IViewProviderContext context)
        {

            // Ensure we explicitly set the featureId
            var feature = await _featureFacade.GetFeatureByIdAsync("Plato.Discuss.Categories");
            if (feature == null)
            {
                return default(IViewProviderResult);
            }

            var categories = await _categoryStore.GetByFeatureIdAsync(feature.Id);

            CategoryBase categoryBase = null;
            if (channelAdmin?.Id > 0)
            {
                categoryBase = await _categoryStore.GetByIdAsync(channelAdmin.Id);
            }

            // channel filter options
            var channelViewOpts = new CategoryIndexOptions
            {
                ChannelId = categoryBase?.Id ?? 0
            };
            
            // Get topic index view model from context
            var viewModel = context.Controller.HttpContext.Items[typeof(EntityIndexViewModel<Topic>)] as EntityIndexViewModel<Topic>;
            if (viewModel == null)
            {
                throw new Exception($"A view model of type {typeof(EntityIndexViewModel<Topic>).ToString()} has not been registered on the HttpContext!");
            }
            
            var indexViewModel = new CategoryIndexViewModel()
            {
                Options = channelViewOpts,
                EntityIndexOptions = viewModel?.Options,
                Pager = viewModel?.Pager
            };

            return Views(
                View<CategoryBase>("Home.Index.Header", model => categoryBase).Zone("header").Order(1),
                View<CategoryBase>("Home.Index.Tools", model => categoryBase).Zone("tools").Order(1),
                View<CategoryIndexViewModel>("Home.Index.Content", model => indexViewModel).Zone("content").Order(1),
                View<CategoryListViewModel<ChannelAdmin>>("Topic.Channels.Index.Sidebar", model =>
                {
                    //model.SelectedChannelId = channel?.Id ?? 0;
                    model.Options = channelViewOpts;
                    model.Categories = categories;
                    return model;
                }).Zone("sidebar").Order(1)
            );

        }

        public override Task<IViewProviderResult> BuildDisplayAsync(Channel indexViewModel, IViewProviderContext updater)
        {
            return Task.FromResult(default(IViewProviderResult));
        }

        public override Task<IViewProviderResult> BuildEditAsync(Channel category, IViewProviderContext updater)
        {
            return Task.FromResult(default(IViewProviderResult));
        }

        public override Task<IViewProviderResult> BuildUpdateAsync(Channel category, IViewProviderContext updater)
        {
            return Task.FromResult(default(IViewProviderResult));
        }
        
    }
}