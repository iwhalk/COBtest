﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Shared.Models
{
    public partial class PropertyType
    {
        public PropertyType()
        {
            Properties = new HashSet<Property>();
        }

        public int IdPropertyType { get; set; }
        public string PropertyTypeName { get; set; }

        public virtual ICollection<Property> Properties { get; set; }
    }
}