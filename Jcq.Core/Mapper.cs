// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Mapper.cs" company="Jan-Cornelius Molnar">
// Copyright 2008-2015 Jan Molnar <jan.molnar@me.com>
// 
// This file is part of JCQ.
// JCQ is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your [option]) any later version.
// JCQ is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with JCQ. If not, see <http://www.gnu.org/licenses/>.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using JCsTools.Core.Configuration;
using JCsTools.Core.Interfaces;

namespace JCsTools.Core
{
    public class Mapper : IMapper
    {
        private readonly Dictionary<Type, Type> _Mappings = new Dictionary<Type, Type>();

        public Mapper()
        {
            _Mappings = new Dictionary<Type, Type>();

            LoadMappings();
        }

        public I CreateImplementation<I>(params Object[] args)
        {
            return (I) CreateImplementation(typeof (I), args);
        }

        public object CreateImplementation(Type interfaceType, params object[] args)
        {
            Type mType;

            mType = GetImplementationType(interfaceType);

            if (mType == null)
                throw new ImplementationNotFoundException(interfaceType);

            return Activator.CreateInstance(mType, args);
        }

        public Type GetImplementationType<I>()
        {
            return GetImplementationType(typeof (I));
        }

        public Type GetImplementationType(Type interfaceType)
        {
            Type mappedType = null;

            if (interfaceType.IsInterface)
            {
                if (!_Mappings.ContainsKey(interfaceType))
                {
                    throw new ImplementationNotFoundException(interfaceType);
                }
                mappedType = _Mappings[interfaceType];
            }
            else
            {
                mappedType = interfaceType;
            }

            return mappedType;
        }

        public bool ExistsImplementation<I>()
        {
            return ExistsImplementation(typeof (I));
        }

        public bool ExistsImplementation(Type interfaceType)
        {
            return _Mappings.ContainsKey(interfaceType);
        }

        public void AddImplementationMapping(Type contractType, Type implementationType)
        {
            lock (_Mappings)
            {
                _Mappings.Add(contractType, implementationType);
            }
        }

        private void LoadMappings()
        {
            MicrokernelSection mkSection;

            mkSection = (MicrokernelSection) ConfigurationManager.GetSection("Microkernel");

            if (mkSection == null)
                throw new ConfigSectionNotFoundException("Microkernel");

            foreach (MappingConfigElement element in mkSection.References)
            {
                Type interfaceType;
                Type mappingType;

                interfaceType = Type.GetType(element.InterfaceType, true, true);
                mappingType = Type.GetType(element.MappingType, true, true);

                _Mappings.Add(interfaceType, mappingType);
            }
        }
    }
}