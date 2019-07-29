using Glass.Mapper.Sc.Web;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Pipelines.HttpRequest;
using Sitecore.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace GlassDemo.Project.Demo.Pipeline
{
	public class NotFoundProcessor : HttpRequestProcessor
	{

		public override void Process(HttpRequestArgs args)
		{
            var requestContext = ServiceLocator.ServiceProvider.GetService<IRequestContext>();

            if (IsValidItem())
			{
				return;
			}
			Context.Item = requestContext.GetHomeItem<Item>();
			
		}

		public bool IsValidItem()
		{
			return Context.Site != null
                && Context.Item != null
                && Context.Item.Visualization.Layout != null; ;
		}
	}
}