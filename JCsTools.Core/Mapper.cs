//    Copyright 2008 Jan Molnar <jan.molnar@abds.de>
//
//    This file is part of JCQ.
//    JCQ is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 2 of the License, or
//    (at your [option]) any later version.
//    JCQ is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//    You should have received a copy of the GNU General Public License
//    along with JCQ. If not, see <http://www.gnu.org/licenses/>.
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace JCsTools.Core
{
  public class Mapper : Interfaces.IMapper
  {
    private System.Collections.Generic.Dictionary<System.Type, System.Type> _Mappings = new System.Collections.Generic.Dictionary<System.Type, System.Type>();

    public Mapper()
    {
      _Mappings = new System.Collections.Generic.Dictionary<System.Type, System.Type>();

      LoadMappings();
    }

    private void LoadMappings()
    {
      JCsTools.Core.Configuration.MicrokernelSection mkSection;

      mkSection = (Core.Configuration.MicrokernelSection)System.Configuration.ConfigurationManager.GetSection("Microkernel");

      if (mkSection == null)
        throw new ConfigSectionNotFoundException("Microkernel");

      foreach (Configuration.MappingConfigElement element in mkSection.References) {
        Type interfaceType;
        Type mappingType;

        interfaceType = Type.GetType(element.InterfaceType, true, true);
        mappingType = Type.GetType(element.MappingType, true, true);

        _Mappings.Add(interfaceType, mappingType);
      }
    }

    public I Interfaces.IMapper.CreateImplementation<I>(params System.Object[] args)
    {
      return (I)CreateImplementation(typeof(I), args);
    }

    public object Interfaces.IMapper.CreateImplementation(System.Type interfaceType, params object[] args)
    {
      System.Type mType;

      mType = GetImplementationType(interfaceType);

      if (mType == null)
        throw new Interfaces.ImplementationNotFoundException(interfaceType);

      return System.Activator.CreateInstance(mType, args);
    }

    public System.Type Interfaces.IMapper.GetImplementationType<I>()
    {
      return GetImplementationType(typeof(I));
    }

    public System.Type Interfaces.IMapper.GetImplementationType(System.Type interfaceType)
    {
      System.Type mappedType = null;

      if (interfaceType.IsInterface) {
        if (!_Mappings.ContainsKey(interfaceType)) {
          throw new Interfaces.ImplementationNotFoundException(interfaceType);
        } else {
          mappedType = _Mappings(interfaceType);
        }
      } else {
        mappedType = interfaceType;
      }

      return mappedType;
    }

    public bool Interfaces.IMapper.ExistsImplementation<I>()
    {
      return ExistsImplementation(typeof(I));
    }

    public bool Interfaces.IMapper.ExistsImplementation(System.Type interfaceType)
    {
      return _Mappings.ContainsKey(interfaceType);
    }

    public void Interfaces.IMapper.AddImplementationMapping(System.Type contractType, System.Type implementationType)
    {
      lock (_Mappings) {
        _Mappings.Add(contractType, implementationType);
      }
    }
  }

}

