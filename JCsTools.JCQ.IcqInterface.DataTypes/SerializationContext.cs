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
namespace JCsTools.JCQ.IcqInterface.DataTypes
{
  public sealed class SerializationContext
  {
    private static readonly Type _ContextType = typeof(SerializationContext);
    private static readonly string _AssemblyName = _ContextType.Assembly.GetName.Name;

    private SerializationContext()
    {

    }

    private static Type GetSnacMapping(SnacDescriptor desc)
    {
      string typeName;
      Type snacType;

      typeName = string.Format("{1}.Snac{0}", SnacDescriptor.GetKey(desc).Replace(',', ""), _AssemblyName);
      snacType = Type.GetType(typeName, false, true);

      return snacType;
    }

    public static Snac DeserializeSnac(int offset, SnacDescriptor desc, List<byte> bytes)
    {
      List<byte> data = bytes.GetRange(offset, bytes.Count - offset);

      Type type;
      Snac x;

      type = GetSnacMapping(desc);

      if (type == null)
        return null;

      x = (Snac)Activator.CreateInstance(type);
      x.Deserialize(data);

      return x;
    }
  }
}
