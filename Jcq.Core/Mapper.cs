// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Mapper.cs" company="Jan-Cornelius Molnar">
// The MIT License (MIT)
// 
// Copyright (c) 2015 Jan-Cornelius Molnar
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using Jcq.Core.Configuration;
using Jcq.Core.Contracts;

namespace Jcq.Core
{
    public class Mapper : IMapper
    {
        private readonly Dictionary<Type, Type> _mappings;

        public Mapper()
        {
            _mappings = new Dictionary<Type, Type>();

            LoadMappings();
        }

        public T CreateImplementation<T>(params object[] args)
        {
            return (T) CreateImplementation(typeof(T), args);
        }

        public object CreateImplementation(Type contractType, params object[] args)
        {
            Type mType = GetImplementationType(contractType);

            if (mType == null)
                throw new ImplementationNotFoundException(contractType);

            return Activator.CreateInstance(mType, args);
        }

        public Type GetImplementationType<I>()
        {
            return GetImplementationType(typeof(I));
        }

        public Type GetImplementationType(Type contractType)
        {
            Type mappedType;

            if (contractType.IsInterface)
            {
                if (!_mappings.ContainsKey(contractType))
                {
                    throw new ImplementationNotFoundException(contractType);
                }
                mappedType = _mappings[contractType];
            }
            else
            {
                mappedType = contractType;
            }

            return mappedType;
        }

        public bool ExistsContractImplementation<I>()
        {
            return ExistsContractImplementation(typeof(I));
        }

        public bool ExistsContractImplementation(Type contractType)
        {
            return _mappings.ContainsKey(contractType);
        }

        public void RegisterContractImplementation(Type contractType, Type implementationType)
        {
            lock (_mappings)
            {
                _mappings.Add(contractType, implementationType);
            }
        }

        private void LoadMappings()
        {
            var mkSection = (MicrokernelSection) ConfigurationManager.GetSection("Microkernel");

            if (mkSection == null)
                throw new ConfigSectionNotFoundException("Microkernel");

            foreach (MappingConfigElement element in mkSection.References)
            {
                Type interfaceType = Type.GetType(element.InterfaceType, true, true);
                Type mappingType = Type.GetType(element.MappingType, true, true);

                _mappings.Add(interfaceType, mappingType);
            }
        }
    }
}