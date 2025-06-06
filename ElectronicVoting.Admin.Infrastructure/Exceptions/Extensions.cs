﻿using Microsoft.Extensions.DependencyInjection;

namespace ElectronicVoting.Admin.Infrastructure.Exceptions;

public static class Extensions
{
    public static IServiceCollection AddGlobalException(this IServiceCollection service)
    {
        service.AddProblemDetails();
        service.AddExceptionHandler<GlobalExceptionHandler>();
        return service;
    }
}