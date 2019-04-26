﻿using System;
using System.Threading.Tasks;
using Plato.Internal.Layout.ViewProviders;
using Plato.Metrics.Models;
using Plato.Reports.Models;
using Plato.Reports.ViewModels;

namespace Plato.Reports.ViewProviders
{
    public class PageViewsViewProvider : BaseViewProvider<PageViewsReport>
    {

        public override Task<IViewProviderResult> BuildIndexAsync(PageViewsReport viewReportModel, IViewProviderContext context)
        {

            // Get view model from context
            var indexViewModel =
                context.Controller.HttpContext.Items[typeof(ReportIndexViewModel<Metric>)] as ReportIndexViewModel<Metric>;
            
            // Ensure we have the view model
            if (indexViewModel == null)
            {
                throw new Exception("No type of ReportIndexViewModel has been registered with HttpContext.Items");
            }

            return Task.FromResult(Views(
                View<ReportIndexViewModel<Metric>>("Admin.PageViews.Header", model => indexViewModel).Zone("header").Order(1),
                View<ReportIndexViewModel<Metric>>("Admin.PageViews.Tools", model => indexViewModel).Zone("tools").Order(1),
                View<ReportIndexViewModel<Metric>>("Admin.PageViews.Content", model => indexViewModel).Zone("content").Order(1)
            ));
        }
        
        public override Task<IViewProviderResult> BuildDisplayAsync(PageViewsReport viewReportModel, IViewProviderContext context)
        {
            return Task.FromResult(default(IViewProviderResult));
        }
        
        public override Task<IViewProviderResult> BuildEditAsync(PageViewsReport viewReportModel, IViewProviderContext context)
        {
            return Task.FromResult(default(IViewProviderResult));
        }

        public override Task<IViewProviderResult> BuildUpdateAsync(PageViewsReport viewReportModel, IViewProviderContext context)
        {
            return Task.FromResult(default(IViewProviderResult));
        }

    }

}
