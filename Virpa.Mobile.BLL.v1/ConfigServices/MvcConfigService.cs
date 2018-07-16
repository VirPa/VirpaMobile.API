using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Virpa.Mobile.BLL.v1.ConfigServices {

    public static class MvcConfigService {

        public static IServiceCollection RegisterMvc(this IServiceCollection service) {

            service.AddMvc()
                .AddMvcOptions(option => option.MvcOption())
                .AddJsonOptions(option => option.MvcJsonOption());

            return service;
        }

        private static MvcOptions MvcOption(this MvcOptions option) {

            option.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());

            return option;
        }

        private static MvcJsonOptions MvcJsonOption(this MvcJsonOptions option) {

            option.SerializerSettings.Formatting = Formatting.Indented;
            option.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            return option;
        }
    }

}