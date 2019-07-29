using demo.portal.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace GlassDemo.Project.Demo
{
	public class GlassCustomConfigurator : IServicesConfigurator
	{
		public void Configure(IServiceCollection serviceCollection)
		{
			serviceCollection.AddTransient<HomeController>();
		}
	}
}