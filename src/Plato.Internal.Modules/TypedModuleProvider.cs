﻿using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.Extensions.DependencyModel;
using Plato.Internal.Models.Modules;
using Plato.Internal.Modules.Abstractions;

namespace Plato.Internal.Modules
{

    public class TypedModuleProvider : ITypedModuleProvider
    {

        private readonly ConcurrentDictionary<Type, IModuleEntry> _modules
            = new ConcurrentDictionary<Type, IModuleEntry>();

        private readonly IModuleManager _moduleManager;

        public TypedModuleProvider(
            IModuleManager moduleManager)
        {
            _moduleManager = moduleManager;
        }
        
        public async Task<IModuleEntry> GetModuleForDependency(Type dependency)
        {
            
            var moduleEntries = await _moduleManager.LoadModulesAsync();
            foreach (var moduleEntry in moduleEntries)
            {
                if (dependency.Assembly.FullName == moduleEntry.Assembly.FullName)
                {
                    return moduleEntry;
                }
            }
            
            throw new InvalidOperationException($"Could not resolve module for type {dependency.Name}");
        }

        public async Task<IDictionary<Type, IModuleEntry>> GetModuleDependenciesAsync(IEnumerable<IModuleEntry> modules)
        {

            await BuildTypedProvider();

            var entries = new Dictionary<Type, IModuleEntry>();
            if (modules != null)
            {
                foreach (var module in modules)
                {
                    var types = _modules
                        .Where(m => m.Value.Descriptor.Id == module.Descriptor.Id)
                        .Select(m => m.Key);
                    foreach (var type in types)
                    {
                        if (!entries.ContainsKey(type))
                        {
                            entries.Add(type, module);
                        }
                    }
                }
            }

            return entries;

        }

        public async Task<Type> GetTypeCandidateAsync(string typeName, Type baseType)
        {

            //var assemblies = DependencyContext.Default.RuntimeLibraries;
            //foreach (var candidate in assemblies
            //    .Where(a => baseType.IsAssignableFrom())
            //    .Select(t => t.Key.GetTypeInfo()))
            //{
            //    if (candidate.GetTypeInfo().FullName == typeName)
            //    {
            //        return candidate.AsType();
            //    }
            //}

            // If type is not found within DI attempt to 
            // locate the type within dynamically loaded modules

            await BuildTypedProvider();

            foreach (var candidate in _modules
                .Where(p => baseType.IsAssignableFrom(p.Key))
                .Select(t => t.Key.GetTypeInfo()))
            {
                if (candidate.GetTypeInfo().FullName == typeName)
                {
                    return candidate.AsType();
                }
            }

            return null;


        }

        async Task BuildTypedProvider()
        {
            var moduleEntries = await _moduleManager.LoadModulesAsync();
            foreach (var moduleEntry in moduleEntries)
            {
                // Get all valid types from module
                var types = moduleEntry.Assmeblies.SelectMany(assembly =>
                    assembly.ExportedTypes.Where(IsComponentType));
                foreach (var type in types)
                {
                    TryAdd(type, moduleEntry);
                }
            }
        }

        bool IsComponentType(Type type)
        {
            if (type == null)
                return false;
            return type.IsClass && !type.IsAbstract && type.IsPublic;
        }

        void TryAdd(Type type, IModuleEntry module)
        {
            _modules.TryAdd(type, module);
        }

    }
}
