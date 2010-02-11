﻿#region License

// Copyright 2010 Buu Nguyen, Morten Mertner
// 
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
// http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.
// 
// The latest version of this file can be found at http://fasterflect.codeplex.com/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fasterflect;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FasterflectTest.SampleModel.Animals;

namespace FasterflectTest.Lookup
{
    [TestClass]
    public class PropertyTest : BaseLookupTest
	{
        #region Single Property
        [TestMethod]
		public void TestPropertyInstance()
        {
			PropertyInfo property = typeof(object).Property( "ID" );
			Assert.IsNull( property );

			AnimalInstancePropertyNames.Select( s => typeof(Animal).Property( s ) ).ForEach( Assert.IsNotNull );
			LionInstancePropertyNames.Select( s => typeof(Lion).Property( s ) ).ForEach( Assert.IsNotNull );
        }

        [TestMethod]
		public void TestPropertyInstanceIgnoreCase()
        {
        	BindingFlags flags = Flags.InstanceCriteria | BindingFlags.IgnoreCase;

			AnimalInstancePropertyNames.Select( s => s.ToLower() ).Select( s => typeof(Animal).Property( s ) ).ForEach( Assert.IsNull );
			AnimalInstancePropertyNames.Select( s => s.ToLower() ).Select( s => typeof(Animal).Property( s, flags ) ).ForEach( Assert.IsNotNull );

			LionInstancePropertyNames.Select( s => s.ToLower() ).Select( s => typeof(Lion).Property( s ) ).ForEach( Assert.IsNull );
			LionInstancePropertyNames.Select( s => s.ToLower() ).Select( s => typeof(Lion).Property( s, flags ) ).ForEach( Assert.IsNotNull );
        }

        [TestMethod]
		public void TestPropertyInstanceWithPropertyType()
        {
        	BindingFlags flags = Flags.InstanceCriteria;

			AnimalInstancePropertyNames.Select( s => typeof(Animal).Property( s, flags, AnimalInstancePropertyTypes[ Array.IndexOf( AnimalInstancePropertyNames, s ) ] ) ).ForEach( Assert.IsNotNull );
			LionInstancePropertyNames.Select( s => typeof(Lion).Property( s, flags, LionInstancePropertyTypes[ Array.IndexOf( LionInstancePropertyNames, s ) ] ) ).ForEach( Assert.IsNotNull );
        }

        [TestMethod]
		public void TestPropertyInstanceDeclaredOnly()
        {
        	BindingFlags flags = Flags.InstanceCriteria | BindingFlags.DeclaredOnly;
			
			AnimalInstancePropertyNames.Select( s => typeof(Animal).Property( s, flags ) ).ForEach( Assert.IsNotNull );
			LionDeclaredInstancePropertyNames.Select( s => typeof(Lion).Property( s, flags ) ).ForEach( Assert.IsNotNull );
        }

        [TestMethod]
		public void TestPropertyStatic()
        {
        	BindingFlags flags = Flags.StaticCriteria;
			
			AnimalInstancePropertyNames.Select( s => typeof(Animal).Property( s, flags ) ).ForEach( Assert.IsNull );

			AnimalStaticPropertyNames.Select( s => typeof(Animal).Property( s, flags ) ).ForEach( Assert.IsNotNull );
			AnimalStaticPropertyNames.Select( s => typeof(Lion).Property( s, flags ) ).ForEach( Assert.IsNotNull );
        }

        [TestMethod]
		public void TestPropertyStaticDeclaredOnly()
        {
        	BindingFlags flags = Flags.StaticCriteria | BindingFlags.DeclaredOnly;
			
			AnimalStaticPropertyNames.Select( s => typeof(Animal).Property( s, flags ) ).ForEach( Assert.IsNotNull );
			AnimalStaticPropertyNames.Select( s => typeof(Lion).Property( s, flags ) ).ForEach( Assert.IsNull );
        }
		#endregion

		#region Multiple Properties
        [TestMethod]
		public void TestPropertiesInstance()
        {
			IList<PropertyInfo> properties = typeof(object).Properties();
			Assert.IsNotNull( properties );
			Assert.AreEqual( 0, properties.Count );

			properties = typeof(Animal).Properties();
			Assert.AreEqual( AnimalInstanceFieldNames.Length, properties.Count );
			Assert.IsTrue( AnimalInstancePropertyNames.SequenceEqual( properties.Select( p => p.Name ) ) );
			Assert.IsTrue( AnimalInstancePropertyTypes.SequenceEqual( properties.Select( p => p.PropertyType ) ) );

			properties = typeof(Mammal).Properties();
			Assert.AreEqual( AnimalInstanceFieldNames.Length, properties.Count );

			properties = typeof(Lion).Properties();
			Assert.AreEqual( LionInstancePropertyNames.Length, properties.Count );
			Assert.IsTrue( LionInstancePropertyNames.SequenceEqual( properties.Select( p => p.Name ) ) );
			Assert.IsTrue( LionInstancePropertyTypes.SequenceEqual( properties.Select( p => p.PropertyType ) ) );
        }

        [TestMethod]
		public void TestPropertiesInstanceWithDeclaredOnlyFlag()
        {
			IList<PropertyInfo> properties = typeof(object).Properties( Flags.InstanceCriteria | BindingFlags.DeclaredOnly );
			Assert.IsNotNull( properties );
			Assert.AreEqual( 0, properties.Count );

			properties = typeof(Animal).Properties( Flags.InstanceCriteria | BindingFlags.DeclaredOnly );
			Assert.AreEqual( AnimalInstancePropertyNames.Length, properties.Count );
        	var x = properties.Select( p => p.Name );
			Assert.IsTrue( AnimalInstancePropertyNames.SequenceEqual( properties.Select( p => p.Name ) ) );
			Assert.IsTrue( AnimalInstancePropertyTypes.SequenceEqual( properties.Select( p => p.PropertyType ) ) );

			properties = typeof(Mammal).Properties( Flags.InstanceCriteria | BindingFlags.DeclaredOnly );
			Assert.AreEqual( 0, properties.Count );

			properties = typeof(Lion).Properties( Flags.InstanceCriteria | BindingFlags.DeclaredOnly );
			Assert.AreEqual( LionDeclaredInstancePropertyNames.Length, properties.Count );
			Assert.IsTrue( LionDeclaredInstancePropertyNames.SequenceEqual( properties.Select( p => p.Name ) ) );
			Assert.IsTrue( LionDeclaredInstancePropertyTypes.SequenceEqual( properties.Select( p => p.PropertyType ) ) );
        }

        [TestMethod]
		public void TestPropertiesStatic()
        {
			IList<PropertyInfo> properties = typeof(object).Properties( Flags.StaticCriteria );
			Assert.IsNotNull( properties );
			Assert.AreEqual( 0, properties.Count );

			properties = typeof(Animal).Properties( Flags.StaticCriteria );
			Assert.AreEqual( AnimalStaticPropertyNames.Length, properties.Count );
			Assert.IsTrue( AnimalStaticPropertyNames.SequenceEqual( properties.Select( p => p.Name ) ) );
			Assert.IsTrue( AnimalStaticPropertyTypes.SequenceEqual( properties.Select( p => p.PropertyType ) ) );

			properties = typeof(Mammal).Properties( Flags.StaticCriteria );
			Assert.AreEqual( AnimalStaticPropertyNames.Length, properties.Count );

			properties = typeof(Lion).Properties( Flags.StaticCriteria );
			Assert.AreEqual( AnimalStaticPropertyNames.Length, properties.Count );
			Assert.IsTrue( AnimalStaticPropertyNames.SequenceEqual( properties.Select( p => p.Name ) ) );
			Assert.IsTrue( AnimalStaticPropertyTypes.SequenceEqual( properties.Select( p => p.PropertyType ) ) );
       }

        [TestMethod]
		public void TestPropertiesStaticWithDeclaredOnlyFlag()
        {
			IList<PropertyInfo> properties = typeof(object).Properties( Flags.StaticCriteria | BindingFlags.DeclaredOnly );
			Assert.IsNotNull( properties );
			Assert.AreEqual( 0, properties.Count );

			properties = typeof(Animal).Properties( Flags.StaticCriteria | BindingFlags.DeclaredOnly );
			Assert.AreEqual( AnimalStaticPropertyNames.Length, properties.Count );
			Assert.IsTrue( AnimalStaticPropertyNames.SequenceEqual( properties.Select( p => p.Name ) ) );
			Assert.IsTrue( AnimalStaticPropertyTypes.SequenceEqual( properties.Select( p => p.PropertyType ) ) );

			properties = typeof(Mammal).Properties( Flags.StaticCriteria | BindingFlags.DeclaredOnly );
			Assert.AreEqual( 0, properties.Count );

			properties = typeof(Lion).Properties( Flags.StaticCriteria | BindingFlags.DeclaredOnly );
			Assert.AreEqual( 0, properties.Count );
        }
		
		[TestMethod]
		public void TestPropertiesWithNameFilterList()
        {
			IList<PropertyInfo> properties = typeof(object).Properties( AnimalInstancePropertyNames );
			Assert.AreEqual( 0, properties.Count );

			properties = typeof(Animal).Properties( AnimalInstancePropertyNames );
			Assert.AreEqual( AnimalInstanceFieldNames.Length, properties.Count );
			Assert.IsTrue( AnimalInstancePropertyNames.SequenceEqual( properties.Select( p => p.Name ) ) );

			properties = typeof(Lion).Properties( AnimalInstancePropertyNames );
			Assert.AreEqual( AnimalInstancePropertyNames.Length, properties.Count );
			Assert.IsTrue( AnimalInstancePropertyNames.SequenceEqual( properties.Select( p => p.Name ) ) );
			Assert.IsTrue( AnimalInstancePropertyTypes.SequenceEqual( properties.Select( p => p.PropertyType ) ) );

			var list = AnimalInstancePropertyNames.Where( s => s.Contains( "C" ) ).ToArray();
			properties = typeof(Animal).Properties( list );
			Assert.AreEqual( list.Length, properties.Count );
			Assert.IsTrue( list.SequenceEqual( properties.Select( p => p.Name ) ) );

			list = AnimalInstancePropertyNames.Where( s => s.Contains( "B" ) ).ToArray();
			properties = typeof(Lion).Properties( list );
			Assert.AreEqual( list.Length, properties.Count );
			Assert.IsTrue( list.SequenceEqual( properties.Select( p => p.Name ) ) );
        }
		#endregion
	}
}
