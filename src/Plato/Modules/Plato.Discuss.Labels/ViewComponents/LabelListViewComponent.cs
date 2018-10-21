﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Plato.Discuss.Labels.Models;
using Plato.Discuss.Labels.ViewModels;
using Plato.Internal.Features.Abstractions;
using Plato.Internal.Navigation;
using Plato.Labels.Stores;

namespace Plato.Discuss.Labels.ViewComponents
{

    public class LabelListViewComponent : ViewComponent
    {

        private readonly ILabelStore<Label> _labelStore;
        private readonly IFeatureFacade _featureFacade;

        public LabelListViewComponent(
            ILabelStore<Label> labelStore, 
            IFeatureFacade featureFacade)
        {
            _labelStore = labelStore;
            _featureFacade = featureFacade;
        }

        public async Task<IViewComponentResult> InvokeAsync(
            LabelIndexOptions options,
            PagerOptions pager)
        {

            if (options == null)
            {
                options = new LabelIndexOptions();
            }

            if (pager == null)
            {
                pager = new PagerOptions();
            }

            var model = await GetViewModel(options, pager);

            return View(model);

        }

        async Task<LabelIndexViewModel> GetViewModel(
            LabelIndexOptions options,
            PagerOptions pager)
        {

            var feature = await _featureFacade.GetFeatureByIdAsync("Plato.Discuss.Labels");

            var labels = await _labelStore.QueryAsync()
                .Take(pager.Page, pager.PageSize)
                .Select<LabelQueryParams>(q =>
                {
                    if (feature != null)
                    {
                        q.FeatureId.Equals(feature.Id);
                    }

                    if (!String.IsNullOrEmpty(options.Search))
                    {
                        q.Name.Like(options.Search).Or();
                        q.Description.Like(options.Search);
                    }
                })
                .OrderBy(options.Sort.ToString(), options.Order)
                .ToList();
            
            return new LabelIndexViewModel(
                labels,
                options,
                pager);
        }

    }


}