using Autofac;
using CVirtual.DataAccess.SQLServer.Context;
using CVirtual.DataAccess.SQLServer.IQueries;
using CVirtual.DataAccess.SQLServer.Queries;
using CVirtual.Domain.Contract;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.CrossCutting
{
    public class ContextDbModule : Autofac.Module
    {
        public static IConfiguration Configuration;

        public ContextDbModule(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            string connectionString = Configuration.GetSection("ConnectionStrings:CVDBContext").Value;
            string context = "contextSeguridad";    
            

            builder.RegisterType<SeguridadDbContext>().Named<ISeguridadDbContext>(context).WithParameter("connstr", connectionString).InstancePerLifetimeScope();

            builder.RegisterType<UsuarioQuery>().As<IUsuarioQuery>().WithParameter((c, p) => true, (c, p) => p.ResolveNamed<ISeguridadDbContext>(context));
            builder.RegisterType<AdminQuery>().As<IAdminQuery>().WithParameter((c, p) => true, (c, p) => p.ResolveNamed<ISeguridadDbContext>(context));
            builder.RegisterType<ModuloQuery>().As<IModuloQuery>().WithParameter((c, p) => true, (c, p) => p.ResolveNamed<ISeguridadDbContext>(context));
            builder.RegisterType<CategoriaQuery>().As<ICategoriaQuery>().WithParameter((c, p) => true, (c, p) => p.ResolveNamed<ISeguridadDbContext>(context));
            builder.RegisterType<SubcategoriaQuery>().As<ISubcategoriaQuery>().WithParameter((c, p) => true, (c, p) => p.ResolveNamed<ISeguridadDbContext>(context));


            //builder.RegisterType<HistoricoPersonaQuery>().As<IHistoricoPersonaQuery>().WithParameter((c, p) => true, (c, p) => p.ResolveNamed<ISeguridadDbContext>(context));

            //builder.RegisterType<PersonaQuery>().As<IPersonaQuery>().WithParameter((c, p) => true, (c, p) => p.ResolveNamed<ISeguridadDbContext>(context));

            //builder.RegisterType<PostulanteQuery>().As<IPostulanteQuery>().WithParameter((c, p) => true, (c, p) => p.ResolveNamed<ISeguridadDbContext>(context));

            //builder.RegisterType<CargaMasivaQuery>().As<ICargaMasivaQuery>().WithParameter((c, p) => true, (c, p) => p.ResolveNamed<ISeguridadDbContext>(context));


            builder.RegisterAssemblyTypes(Assembly.Load(new AssemblyName("CVirtual.Application")))
                .Where(t => t.Name.EndsWith("Service", StringComparison.Ordinal) && t.GetTypeInfo().IsClass)
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(Assembly.Load(new AssemblyName("CVirtual.Application")))
                .Where(t => t.Name.EndsWith("Config", StringComparison.Ordinal) && t.GetTypeInfo().IsClass)
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(Assembly.Load(new AssemblyName("CVirtual.Application")))
                .Where(t => t.Name.EndsWith("Security", StringComparison.Ordinal) && t.GetTypeInfo().IsClass)
                .AsImplementedInterfaces();

        }
    }
}
