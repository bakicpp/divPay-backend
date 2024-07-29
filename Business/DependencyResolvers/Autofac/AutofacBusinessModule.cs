using System;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;

namespace Business.DependencyResolvers.Autofac
{
	public class AutofacBusinessModule : Module
	{
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<CardManager>().As<ICardService>();
            builder.RegisterType<EfCardDal>().As<ICardDal>();

            builder.RegisterType<ClientManager>().As<IClientService>();
            builder.RegisterType<EfClientDal>().As<IClientDal>();

            builder.RegisterType<PaymentRequestManager>().As<IPaymentRequestService>();
            builder.RegisterType<EfPaymentRequestDal>().As<IPaymentRequestDal>();

            builder.RegisterType<AccountManager>().As<IAccountService>();
            builder.RegisterType<EfAccountDal>().As<IAccountDal>();

            builder.RegisterType<ExpenseManager>().As<IExpenseService>();
            builder.RegisterType<EfExpenseDal>().As<IExpenseDal>();

            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();

        }

    }
}

