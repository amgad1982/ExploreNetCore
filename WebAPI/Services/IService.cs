using System;
using Microsoft.Extensions.DependencyInjection;

namespace WebAPI.Services {
    public interface IService {
        void RegisterService (IServiceCollection services);
    }
}