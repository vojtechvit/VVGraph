﻿using WebServices.DataAccess.Neo4j;
using WebServices.DataAccess.Neo4j.Contracts;
using WebServices.DataAccess.Neo4j.DelegatedAlgorithms;
using WebServices.DataAccess.Neo4j.Repositories;
using Domain.Algorithms.Contracts;
using Domain.Factories;
using Domain.Factories.Contracts;
using Domain.Repositories.Contracts;
using Domain.Validation;
using Domain.Validation.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection
{
    public static class CommonMappings
    {
        public static void AddVVGraphCommon(this IServiceCollection services)
        {
            // Domain Validators
            services.AddSingleton<IGraphValidator, GraphValidator>();
            services.AddSingleton<INodeValidator, NodeValidator>();

            // Domain Factories
            services.AddSingleton<IGraphFactory, GraphFactory>();
            services.AddSingleton<INodeFactory, NodeFactory>();
            services.AddSingleton<IPathFactory, PathFactory>();

            // Domain Repositories
            services.AddSingleton<IGraphRepository, Neo4jGraphRepository>();
            services.AddSingleton<INodeRepository, Neo4jNodeRepository>();

            // Domain Delegated Algorithms
            services.AddSingleton<IPathFinder, Neo4jPathFinder>();

            // Neo4j Infrastructure
            services.AddSingleton<INeo4jDriver, Neo4jDriver>();
        }
    }
}