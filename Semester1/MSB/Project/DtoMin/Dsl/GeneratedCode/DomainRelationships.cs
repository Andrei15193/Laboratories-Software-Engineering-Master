﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using DslModeling = global::Microsoft.VisualStudio.Modeling;
using DslDesign = global::Microsoft.VisualStudio.Modeling.Design;
namespace Andrei15193.DtoMin
{
	/// <summary>
	/// DomainRelationship DTOMapHasDTOs
	/// Embedding relationship between the Model and Elements
	/// </summary>
	[DslDesign::DisplayNameResource("Andrei15193.DtoMin.DTOMapHasDTOs.DisplayName", typeof(global::Andrei15193.DtoMin.DtoMinDomainModel), "Andrei15193.DtoMin.GeneratedCode.DomainModelResx")]
	[DslDesign::DescriptionResource("Andrei15193.DtoMin.DTOMapHasDTOs.Description", typeof(global::Andrei15193.DtoMin.DtoMinDomainModel), "Andrei15193.DtoMin.GeneratedCode.DomainModelResx")]
	[DslModeling::DomainModelOwner(typeof(global::Andrei15193.DtoMin.DtoMinDomainModel))]
	[global::System.CLSCompliant(true)]
	[DslModeling::DomainRelationship(IsEmbedding=true)]
	[DslModeling::DomainObjectId("73a6cdf0-6cc2-4ac3-9452-ad35ff0441b0")]
	public partial class DTOMapHasDTOs : DslModeling::ElementLink
	{
		#region Constructors, domain class Id
		
		/// <summary>
		/// DTOMapHasDTOs domain class Id.
		/// </summary>
		public static readonly new global::System.Guid DomainClassId = new global::System.Guid(0x73a6cdf0, 0x6cc2, 0x4ac3, 0x94, 0x52, 0xad, 0x35, 0xff, 0x04, 0x41, 0xb0);
	
				
		/// <summary>
		/// Constructor
		/// Creates a DTOMapHasDTOs link in the same Partition as the given DTOMap
		/// </summary>
		/// <param name="source">DTOMap to use as the source of the relationship.</param>
		/// <param name="target">DTO to use as the target of the relationship.</param>
		public DTOMapHasDTOs(DTOMap source, DTO target)
			: base((source != null ? source.Partition : null), new DslModeling::RoleAssignment[]{new DslModeling::RoleAssignment(DTOMapHasDTOs.DTOsDomainRoleId, source), new DslModeling::RoleAssignment(DTOMapHasDTOs.DTOMapDomainRoleId, target)}, null)
		{
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="store">Store where new link is to be created.</param>
		/// <param name="roleAssignments">List of relationship role assignments.</param>
		public DTOMapHasDTOs(DslModeling::Store store, params DslModeling::RoleAssignment[] roleAssignments)
			: base(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, roleAssignments, null)
		{
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="store">Store where new link is to be created.</param>
		/// <param name="roleAssignments">List of relationship role assignments.</param>
		/// <param name="propertyAssignments">List of properties assignments to set on the new link.</param>
		public DTOMapHasDTOs(DslModeling::Store store, DslModeling::RoleAssignment[] roleAssignments, DslModeling::PropertyAssignment[] propertyAssignments)
			: base(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, roleAssignments, propertyAssignments)
		{
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="partition">Partition where new link is to be created.</param>
		/// <param name="roleAssignments">List of relationship role assignments.</param>
		public DTOMapHasDTOs(DslModeling::Partition partition, params DslModeling::RoleAssignment[] roleAssignments)
			: base(partition, roleAssignments, null)
		{
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="partition">Partition where new link is to be created.</param>
		/// <param name="roleAssignments">List of relationship role assignments.</param>
		/// <param name="propertyAssignments">List of properties assignments to set on the new link.</param>
		public DTOMapHasDTOs(DslModeling::Partition partition, DslModeling::RoleAssignment[] roleAssignments, DslModeling::PropertyAssignment[] propertyAssignments)
			: base(partition, roleAssignments, propertyAssignments)
		{
		}
		#endregion
		#region DTOs domain role code
		
		/// <summary>
		/// DTOs domain role Id.
		/// </summary>
		public static readonly global::System.Guid DTOsDomainRoleId = new global::System.Guid(0xf023c5e1, 0xdc6a, 0x4042, 0xa5, 0x90, 0xfb, 0xbb, 0xac, 0xa5, 0x60, 0x5c);
		
		/// <summary>
		/// DomainRole DTOs
		/// </summary>
		[DslDesign::DisplayNameResource("Andrei15193.DtoMin.DTOMapHasDTOs/DTOs.DisplayName", typeof(global::Andrei15193.DtoMin.DtoMinDomainModel), "Andrei15193.DtoMin.GeneratedCode.DomainModelResx")]
		[DslDesign::DescriptionResource("Andrei15193.DtoMin.DTOMapHasDTOs/DTOs.Description", typeof(global::Andrei15193.DtoMin.DtoMinDomainModel), "Andrei15193.DtoMin.GeneratedCode.DomainModelResx")]
		[DslModeling::DomainRole(DslModeling::DomainRoleOrder.Source, PropertyName = "DTOs", PropertyDisplayNameKey="Andrei15193.DtoMin.DTOMapHasDTOs/DTOs.PropertyDisplayName",  PropagatesCopy = DslModeling::PropagatesCopyOption.PropagatesCopyToLinkAndOppositeRolePlayer, Multiplicity = DslModeling::Multiplicity.ZeroMany)]
		[DslModeling::DomainObjectId("f023c5e1-dc6a-4042-a590-fbbbaca5605c")]
		public virtual DTOMap DTOs
		{
			[global::System.Diagnostics.DebuggerStepThrough]
			get
			{
				return (DTOMap)DslModeling::DomainRoleInfo.GetRolePlayer(this, DTOsDomainRoleId);
			}
			[global::System.Diagnostics.DebuggerStepThrough]
			set
			{
				DslModeling::DomainRoleInfo.SetRolePlayer(this, DTOsDomainRoleId, value);
			}
		}
				
		#endregion
		#region Static methods to access DTOMap of a DTO
		/// <summary>
		/// Gets DTOMap.
		/// </summary>
		[global::System.Diagnostics.DebuggerStepThrough]
		[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011")]
		public static DTOMap GetDTOMap(DTO element)
		{
			return DslModeling::DomainRoleInfo.GetLinkedElement(element, DTOMapDomainRoleId) as DTOMap;
		}
		
		/// <summary>
		/// Sets DTOMap.
		/// </summary>
		[global::System.Diagnostics.DebuggerStepThrough]
		[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011")]
		public static void SetDTOMap(DTO element, DTOMap newDTOs)
		{
			DslModeling::DomainRoleInfo.SetLinkedElement(element, DTOMapDomainRoleId, newDTOs);
		}
		#endregion
		#region DTOMap domain role code
		
		/// <summary>
		/// DTOMap domain role Id.
		/// </summary>
		public static readonly global::System.Guid DTOMapDomainRoleId = new global::System.Guid(0x061f7842, 0x4217, 0x4050, 0xb7, 0x0a, 0x13, 0xe5, 0xb6, 0x6c, 0x9e, 0x75);
		
		/// <summary>
		/// DomainRole DTOMap
		/// </summary>
		[DslDesign::DisplayNameResource("Andrei15193.DtoMin.DTOMapHasDTOs/DTOMap.DisplayName", typeof(global::Andrei15193.DtoMin.DtoMinDomainModel), "Andrei15193.DtoMin.GeneratedCode.DomainModelResx")]
		[DslDesign::DescriptionResource("Andrei15193.DtoMin.DTOMapHasDTOs/DTOMap.Description", typeof(global::Andrei15193.DtoMin.DtoMinDomainModel), "Andrei15193.DtoMin.GeneratedCode.DomainModelResx")]
		[DslModeling::DomainRole(DslModeling::DomainRoleOrder.Target, PropertyName = "DTOMap", PropertyDisplayNameKey="Andrei15193.DtoMin.DTOMapHasDTOs/DTOMap.PropertyDisplayName", PropagatesDelete = true,  PropagatesCopy = DslModeling::PropagatesCopyOption.DoNotPropagateCopy, Multiplicity = DslModeling::Multiplicity.One)]
		[DslModeling::DomainObjectId("061f7842-4217-4050-b70a-13e5b66c9e75")]
		public virtual DTO DTOMap
		{
			[global::System.Diagnostics.DebuggerStepThrough]
			get
			{
				return (DTO)DslModeling::DomainRoleInfo.GetRolePlayer(this, DTOMapDomainRoleId);
			}
			[global::System.Diagnostics.DebuggerStepThrough]
			set
			{
				DslModeling::DomainRoleInfo.SetRolePlayer(this, DTOMapDomainRoleId, value);
			}
		}
				
		#endregion
		#region Static methods to access DTOs of a DTOMap
		/// <summary>
		/// Gets a list of DTOs.
		/// </summary>
		[global::System.Diagnostics.DebuggerStepThrough]
		[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011")]
		public static DslModeling::LinkedElementCollection<DTO> GetDTOs(DTOMap element)
		{
			return GetRoleCollection<DslModeling::LinkedElementCollection<DTO>, DTO>(element, DTOsDomainRoleId);
		}
		#endregion
		#region DTOs link accessor
		/// <summary>
		/// Get the list of DTOMapHasDTOs links to a DTOMap.
		/// </summary>
		[global::System.Diagnostics.DebuggerStepThrough]
		[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011")]
		public static global::System.Collections.ObjectModel.ReadOnlyCollection<global::Andrei15193.DtoMin.DTOMapHasDTOs> GetLinksToDTOs ( global::Andrei15193.DtoMin.DTOMap dTOsInstance )
		{
			return DslModeling::DomainRoleInfo.GetElementLinks<global::Andrei15193.DtoMin.DTOMapHasDTOs>(dTOsInstance, global::Andrei15193.DtoMin.DTOMapHasDTOs.DTOsDomainRoleId);
		}
		#endregion
		#region DTOMap link accessor
		/// <summary>
		/// Get the DTOMapHasDTOs link to a DTO.
		/// </summary>
		[global::System.Diagnostics.DebuggerStepThrough]
		[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011")]
		public static global::Andrei15193.DtoMin.DTOMapHasDTOs GetLinkToDTOMap (global::Andrei15193.DtoMin.DTO dTOMapInstance)
		{
			global::System.Collections.Generic.IList<global::Andrei15193.DtoMin.DTOMapHasDTOs> links = DslModeling::DomainRoleInfo.GetElementLinks<global::Andrei15193.DtoMin.DTOMapHasDTOs>(dTOMapInstance, global::Andrei15193.DtoMin.DTOMapHasDTOs.DTOMapDomainRoleId);
			global::System.Diagnostics.Debug.Assert(links.Count <= 1, "Multiplicity of DTOMap not obeyed.");
			if ( links.Count == 0 )
			{
				return null;
			}
			else
			{
				return links[0];
			}
		}
		#endregion
		#region DTOMapHasDTOs instance accessors
		
		/// <summary>
		/// Get any DTOMapHasDTOs links between a given DTOMap and a DTO.
		/// </summary>
		[global::System.Diagnostics.DebuggerStepThrough]
		[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011")]
		public static global::System.Collections.ObjectModel.ReadOnlyCollection<global::Andrei15193.DtoMin.DTOMapHasDTOs> GetLinks( global::Andrei15193.DtoMin.DTOMap source, global::Andrei15193.DtoMin.DTO target )
		{
			global::System.Collections.Generic.List<global::Andrei15193.DtoMin.DTOMapHasDTOs> outLinks = new global::System.Collections.Generic.List<global::Andrei15193.DtoMin.DTOMapHasDTOs>();
			global::System.Collections.Generic.IList<global::Andrei15193.DtoMin.DTOMapHasDTOs> links = DslModeling::DomainRoleInfo.GetElementLinks<global::Andrei15193.DtoMin.DTOMapHasDTOs>(source, global::Andrei15193.DtoMin.DTOMapHasDTOs.DTOsDomainRoleId);
			foreach ( global::Andrei15193.DtoMin.DTOMapHasDTOs link in links )
			{
				if ( target.Equals(link.DTOMap) )
				{
					outLinks.Add(link);
				}
			}
			return outLinks.AsReadOnly();
		}
		/// <summary>
		/// Get the one DTOMapHasDTOs link between a given DTOMapand a DTO.
		/// </summary>
		[global::System.Diagnostics.DebuggerStepThrough]
		[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011")]
		public static global::Andrei15193.DtoMin.DTOMapHasDTOs GetLink( global::Andrei15193.DtoMin.DTOMap source, global::Andrei15193.DtoMin.DTO target )
		{
			global::System.Collections.Generic.IList<global::Andrei15193.DtoMin.DTOMapHasDTOs> links = DslModeling::DomainRoleInfo.GetElementLinks<global::Andrei15193.DtoMin.DTOMapHasDTOs>(source, global::Andrei15193.DtoMin.DTOMapHasDTOs.DTOsDomainRoleId);
			foreach ( global::Andrei15193.DtoMin.DTOMapHasDTOs link in links )
			{
				if ( target.Equals(link.DTOMap) )
				{
					return link;
				}
			}
			return null;
		}
		
		#endregion
	}
}
namespace Andrei15193.DtoMin
{
	/// <summary>
	/// DomainRelationship DTOHasAttributes
	/// Description for Andrei15193.DtoMin.DTOHasAttributes
	/// </summary>
	[DslDesign::DisplayNameResource("Andrei15193.DtoMin.DTOHasAttributes.DisplayName", typeof(global::Andrei15193.DtoMin.DtoMinDomainModel), "Andrei15193.DtoMin.GeneratedCode.DomainModelResx")]
	[DslDesign::DescriptionResource("Andrei15193.DtoMin.DTOHasAttributes.Description", typeof(global::Andrei15193.DtoMin.DtoMinDomainModel), "Andrei15193.DtoMin.GeneratedCode.DomainModelResx")]
	[DslModeling::DomainModelOwner(typeof(global::Andrei15193.DtoMin.DtoMinDomainModel))]
	[global::System.CLSCompliant(true)]
	[DslModeling::DomainRelationship(IsEmbedding=true)]
	[DslModeling::DomainObjectId("6ab2d098-dad5-4064-b05b-cb6904569e8b")]
	public partial class DTOHasAttributes : DslModeling::ElementLink
	{
		#region Constructors, domain class Id
		
		/// <summary>
		/// DTOHasAttributes domain class Id.
		/// </summary>
		public static readonly new global::System.Guid DomainClassId = new global::System.Guid(0x6ab2d098, 0xdad5, 0x4064, 0xb0, 0x5b, 0xcb, 0x69, 0x04, 0x56, 0x9e, 0x8b);
	
				
		/// <summary>
		/// Constructor
		/// Creates a DTOHasAttributes link in the same Partition as the given DTO
		/// </summary>
		/// <param name="source">DTO to use as the source of the relationship.</param>
		/// <param name="target">DTOAttribute to use as the target of the relationship.</param>
		public DTOHasAttributes(DTO source, DTOAttribute target)
			: base((source != null ? source.Partition : null), new DslModeling::RoleAssignment[]{new DslModeling::RoleAssignment(DTOHasAttributes.AttributesDomainRoleId, source), new DslModeling::RoleAssignment(DTOHasAttributes.ContainerDomainRoleId, target)}, null)
		{
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="store">Store where new link is to be created.</param>
		/// <param name="roleAssignments">List of relationship role assignments.</param>
		public DTOHasAttributes(DslModeling::Store store, params DslModeling::RoleAssignment[] roleAssignments)
			: base(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, roleAssignments, null)
		{
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="store">Store where new link is to be created.</param>
		/// <param name="roleAssignments">List of relationship role assignments.</param>
		/// <param name="propertyAssignments">List of properties assignments to set on the new link.</param>
		public DTOHasAttributes(DslModeling::Store store, DslModeling::RoleAssignment[] roleAssignments, DslModeling::PropertyAssignment[] propertyAssignments)
			: base(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, roleAssignments, propertyAssignments)
		{
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="partition">Partition where new link is to be created.</param>
		/// <param name="roleAssignments">List of relationship role assignments.</param>
		public DTOHasAttributes(DslModeling::Partition partition, params DslModeling::RoleAssignment[] roleAssignments)
			: base(partition, roleAssignments, null)
		{
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="partition">Partition where new link is to be created.</param>
		/// <param name="roleAssignments">List of relationship role assignments.</param>
		/// <param name="propertyAssignments">List of properties assignments to set on the new link.</param>
		public DTOHasAttributes(DslModeling::Partition partition, DslModeling::RoleAssignment[] roleAssignments, DslModeling::PropertyAssignment[] propertyAssignments)
			: base(partition, roleAssignments, propertyAssignments)
		{
		}
		#endregion
		#region Attributes domain role code
		
		/// <summary>
		/// Attributes domain role Id.
		/// </summary>
		public static readonly global::System.Guid AttributesDomainRoleId = new global::System.Guid(0x2de75d4d, 0x6c82, 0x40a3, 0xba, 0x7b, 0x35, 0x4c, 0x0c, 0x13, 0x3e, 0xe2);
		
		/// <summary>
		/// DomainRole Attributes
		/// Description for Andrei15193.DtoMin.DTOHasAttributes.Attributes
		/// </summary>
		[DslDesign::DisplayNameResource("Andrei15193.DtoMin.DTOHasAttributes/Attributes.DisplayName", typeof(global::Andrei15193.DtoMin.DtoMinDomainModel), "Andrei15193.DtoMin.GeneratedCode.DomainModelResx")]
		[DslDesign::DescriptionResource("Andrei15193.DtoMin.DTOHasAttributes/Attributes.Description", typeof(global::Andrei15193.DtoMin.DtoMinDomainModel), "Andrei15193.DtoMin.GeneratedCode.DomainModelResx")]
		[DslModeling::DomainRole(DslModeling::DomainRoleOrder.Source, PropertyName = "Attributes", PropertyDisplayNameKey="Andrei15193.DtoMin.DTOHasAttributes/Attributes.PropertyDisplayName",  PropagatesCopy = DslModeling::PropagatesCopyOption.PropagatesCopyToLinkAndOppositeRolePlayer, Multiplicity = DslModeling::Multiplicity.ZeroMany)]
		[DslModeling::DomainObjectId("2de75d4d-6c82-40a3-ba7b-354c0c133ee2")]
		public virtual DTO Attributes
		{
			[global::System.Diagnostics.DebuggerStepThrough]
			get
			{
				return (DTO)DslModeling::DomainRoleInfo.GetRolePlayer(this, AttributesDomainRoleId);
			}
			[global::System.Diagnostics.DebuggerStepThrough]
			set
			{
				DslModeling::DomainRoleInfo.SetRolePlayer(this, AttributesDomainRoleId, value);
			}
		}
				
		#endregion
		#region Static methods to access Container of a DTOAttribute
		/// <summary>
		/// Gets Container.
		/// </summary>
		[global::System.Diagnostics.DebuggerStepThrough]
		[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011")]
		public static DTO GetContainer(DTOAttribute element)
		{
			return DslModeling::DomainRoleInfo.GetLinkedElement(element, ContainerDomainRoleId) as DTO;
		}
		
		/// <summary>
		/// Sets Container.
		/// </summary>
		[global::System.Diagnostics.DebuggerStepThrough]
		[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011")]
		public static void SetContainer(DTOAttribute element, DTO newAttributes)
		{
			DslModeling::DomainRoleInfo.SetLinkedElement(element, ContainerDomainRoleId, newAttributes);
		}
		#endregion
		#region Container domain role code
		
		/// <summary>
		/// Container domain role Id.
		/// </summary>
		public static readonly global::System.Guid ContainerDomainRoleId = new global::System.Guid(0xaaeb625a, 0xd78b, 0x41d8, 0xac, 0x7b, 0xc9, 0x7a, 0xf3, 0xf9, 0xdd, 0xde);
		
		/// <summary>
		/// DomainRole Container
		/// Description for Andrei15193.DtoMin.DTOHasAttributes.Container
		/// </summary>
		[DslDesign::DisplayNameResource("Andrei15193.DtoMin.DTOHasAttributes/Container.DisplayName", typeof(global::Andrei15193.DtoMin.DtoMinDomainModel), "Andrei15193.DtoMin.GeneratedCode.DomainModelResx")]
		[DslDesign::DescriptionResource("Andrei15193.DtoMin.DTOHasAttributes/Container.Description", typeof(global::Andrei15193.DtoMin.DtoMinDomainModel), "Andrei15193.DtoMin.GeneratedCode.DomainModelResx")]
		[DslModeling::DomainRole(DslModeling::DomainRoleOrder.Target, PropertyName = "Container", PropertyDisplayNameKey="Andrei15193.DtoMin.DTOHasAttributes/Container.PropertyDisplayName", PropagatesDelete = true,  PropagatesCopy = DslModeling::PropagatesCopyOption.DoNotPropagateCopy, Multiplicity = DslModeling::Multiplicity.One)]
		[DslModeling::DomainObjectId("aaeb625a-d78b-41d8-ac7b-c97af3f9ddde")]
		public virtual DTOAttribute Container
		{
			[global::System.Diagnostics.DebuggerStepThrough]
			get
			{
				return (DTOAttribute)DslModeling::DomainRoleInfo.GetRolePlayer(this, ContainerDomainRoleId);
			}
			[global::System.Diagnostics.DebuggerStepThrough]
			set
			{
				DslModeling::DomainRoleInfo.SetRolePlayer(this, ContainerDomainRoleId, value);
			}
		}
				
		#endregion
		#region Static methods to access Attributes of a DTO
		/// <summary>
		/// Gets a list of Attributes.
		/// </summary>
		[global::System.Diagnostics.DebuggerStepThrough]
		[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011")]
		public static DslModeling::LinkedElementCollection<DTOAttribute> GetAttributes(DTO element)
		{
			return GetRoleCollection<DslModeling::LinkedElementCollection<DTOAttribute>, DTOAttribute>(element, AttributesDomainRoleId);
		}
		#endregion
		#region Attributes link accessor
		/// <summary>
		/// Get the list of DTOHasAttributes links to a DTO.
		/// </summary>
		[global::System.Diagnostics.DebuggerStepThrough]
		[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011")]
		public static global::System.Collections.ObjectModel.ReadOnlyCollection<global::Andrei15193.DtoMin.DTOHasAttributes> GetLinksToAttributes ( global::Andrei15193.DtoMin.DTO attributesInstance )
		{
			return DslModeling::DomainRoleInfo.GetElementLinks<global::Andrei15193.DtoMin.DTOHasAttributes>(attributesInstance, global::Andrei15193.DtoMin.DTOHasAttributes.AttributesDomainRoleId);
		}
		#endregion
		#region Container link accessor
		/// <summary>
		/// Get the DTOHasAttributes link to a DTOAttribute.
		/// </summary>
		[global::System.Diagnostics.DebuggerStepThrough]
		[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011")]
		public static global::Andrei15193.DtoMin.DTOHasAttributes GetLinkToContainer (global::Andrei15193.DtoMin.DTOAttribute containerInstance)
		{
			global::System.Collections.Generic.IList<global::Andrei15193.DtoMin.DTOHasAttributes> links = DslModeling::DomainRoleInfo.GetElementLinks<global::Andrei15193.DtoMin.DTOHasAttributes>(containerInstance, global::Andrei15193.DtoMin.DTOHasAttributes.ContainerDomainRoleId);
			global::System.Diagnostics.Debug.Assert(links.Count <= 1, "Multiplicity of Container not obeyed.");
			if ( links.Count == 0 )
			{
				return null;
			}
			else
			{
				return links[0];
			}
		}
		#endregion
		#region DTOHasAttributes instance accessors
		
		/// <summary>
		/// Get any DTOHasAttributes links between a given DTO and a DTOAttribute.
		/// </summary>
		[global::System.Diagnostics.DebuggerStepThrough]
		[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011")]
		public static global::System.Collections.ObjectModel.ReadOnlyCollection<global::Andrei15193.DtoMin.DTOHasAttributes> GetLinks( global::Andrei15193.DtoMin.DTO source, global::Andrei15193.DtoMin.DTOAttribute target )
		{
			global::System.Collections.Generic.List<global::Andrei15193.DtoMin.DTOHasAttributes> outLinks = new global::System.Collections.Generic.List<global::Andrei15193.DtoMin.DTOHasAttributes>();
			global::System.Collections.Generic.IList<global::Andrei15193.DtoMin.DTOHasAttributes> links = DslModeling::DomainRoleInfo.GetElementLinks<global::Andrei15193.DtoMin.DTOHasAttributes>(source, global::Andrei15193.DtoMin.DTOHasAttributes.AttributesDomainRoleId);
			foreach ( global::Andrei15193.DtoMin.DTOHasAttributes link in links )
			{
				if ( target.Equals(link.Container) )
				{
					outLinks.Add(link);
				}
			}
			return outLinks.AsReadOnly();
		}
		/// <summary>
		/// Get the one DTOHasAttributes link between a given DTOand a DTOAttribute.
		/// </summary>
		[global::System.Diagnostics.DebuggerStepThrough]
		[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011")]
		public static global::Andrei15193.DtoMin.DTOHasAttributes GetLink( global::Andrei15193.DtoMin.DTO source, global::Andrei15193.DtoMin.DTOAttribute target )
		{
			global::System.Collections.Generic.IList<global::Andrei15193.DtoMin.DTOHasAttributes> links = DslModeling::DomainRoleInfo.GetElementLinks<global::Andrei15193.DtoMin.DTOHasAttributes>(source, global::Andrei15193.DtoMin.DTOHasAttributes.AttributesDomainRoleId);
			foreach ( global::Andrei15193.DtoMin.DTOHasAttributes link in links )
			{
				if ( target.Equals(link.Container) )
				{
					return link;
				}
			}
			return null;
		}
		
		#endregion
	}
}
namespace Andrei15193.DtoMin
{
	/// <summary>
	/// DomainRelationship DTOReferencesDTOs
	/// Description for Andrei15193.DtoMin.DTOReferencesDTOs
	/// </summary>
	[DslDesign::DisplayNameResource("Andrei15193.DtoMin.DTOReferencesDTOs.DisplayName", typeof(global::Andrei15193.DtoMin.DtoMinDomainModel), "Andrei15193.DtoMin.GeneratedCode.DomainModelResx")]
	[DslDesign::DescriptionResource("Andrei15193.DtoMin.DTOReferencesDTOs.Description", typeof(global::Andrei15193.DtoMin.DtoMinDomainModel), "Andrei15193.DtoMin.GeneratedCode.DomainModelResx")]
	[DslModeling::DomainModelOwner(typeof(global::Andrei15193.DtoMin.DtoMinDomainModel))]
	[global::System.CLSCompliant(true)]
	[DslModeling::DomainRelationship()]
	[DslModeling::DomainObjectId("6d3941d9-cf31-4248-b831-2cb4cbc497e2")]
	public partial class DTOReferencesDTOs : DslModeling::ElementLink
	{
		#region Constructors, domain class Id
		
		/// <summary>
		/// DTOReferencesDTOs domain class Id.
		/// </summary>
		public static readonly new global::System.Guid DomainClassId = new global::System.Guid(0x6d3941d9, 0xcf31, 0x4248, 0xb8, 0x31, 0x2c, 0xb4, 0xcb, 0xc4, 0x97, 0xe2);
	
				
		/// <summary>
		/// Constructor
		/// Creates a DTOReferencesDTOs link in the same Partition as the given DTO
		/// </summary>
		/// <param name="source">DTO to use as the source of the relationship.</param>
		/// <param name="target">DTO to use as the target of the relationship.</param>
		public DTOReferencesDTOs(DTO source, DTO target)
			: base((source != null ? source.Partition : null), new DslModeling::RoleAssignment[]{new DslModeling::RoleAssignment(DTOReferencesDTOs.ChildsDomainRoleId, source), new DslModeling::RoleAssignment(DTOReferencesDTOs.ParentDomainRoleId, target)}, null)
		{
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="store">Store where new link is to be created.</param>
		/// <param name="roleAssignments">List of relationship role assignments.</param>
		public DTOReferencesDTOs(DslModeling::Store store, params DslModeling::RoleAssignment[] roleAssignments)
			: base(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, roleAssignments, null)
		{
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="store">Store where new link is to be created.</param>
		/// <param name="roleAssignments">List of relationship role assignments.</param>
		/// <param name="propertyAssignments">List of properties assignments to set on the new link.</param>
		public DTOReferencesDTOs(DslModeling::Store store, DslModeling::RoleAssignment[] roleAssignments, DslModeling::PropertyAssignment[] propertyAssignments)
			: base(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, roleAssignments, propertyAssignments)
		{
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="partition">Partition where new link is to be created.</param>
		/// <param name="roleAssignments">List of relationship role assignments.</param>
		public DTOReferencesDTOs(DslModeling::Partition partition, params DslModeling::RoleAssignment[] roleAssignments)
			: base(partition, roleAssignments, null)
		{
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="partition">Partition where new link is to be created.</param>
		/// <param name="roleAssignments">List of relationship role assignments.</param>
		/// <param name="propertyAssignments">List of properties assignments to set on the new link.</param>
		public DTOReferencesDTOs(DslModeling::Partition partition, DslModeling::RoleAssignment[] roleAssignments, DslModeling::PropertyAssignment[] propertyAssignments)
			: base(partition, roleAssignments, propertyAssignments)
		{
		}
		#endregion
		#region Childs domain role code
		
		/// <summary>
		/// Childs domain role Id.
		/// </summary>
		public static readonly global::System.Guid ChildsDomainRoleId = new global::System.Guid(0x89276f33, 0x27dd, 0x4920, 0x94, 0xdc, 0x9a, 0x2e, 0x96, 0x43, 0x3a, 0x61);
		
		/// <summary>
		/// DomainRole Childs
		/// Description for Andrei15193.DtoMin.DTOReferencesDTOs.Childs
		/// </summary>
		[DslDesign::DisplayNameResource("Andrei15193.DtoMin.DTOReferencesDTOs/Childs.DisplayName", typeof(global::Andrei15193.DtoMin.DtoMinDomainModel), "Andrei15193.DtoMin.GeneratedCode.DomainModelResx")]
		[DslDesign::DescriptionResource("Andrei15193.DtoMin.DTOReferencesDTOs/Childs.Description", typeof(global::Andrei15193.DtoMin.DtoMinDomainModel), "Andrei15193.DtoMin.GeneratedCode.DomainModelResx")]
		[DslModeling::DomainRole(DslModeling::DomainRoleOrder.Source, PropertyName = "Childs", PropertyDisplayNameKey="Andrei15193.DtoMin.DTOReferencesDTOs/Childs.PropertyDisplayName",  PropagatesCopy = DslModeling::PropagatesCopyOption.DoNotPropagateCopy, Multiplicity = DslModeling::Multiplicity.ZeroMany)]
		[DslModeling::DomainObjectId("89276f33-27dd-4920-94dc-9a2e96433a61")]
		public virtual DTO Childs
		{
			[global::System.Diagnostics.DebuggerStepThrough]
			get
			{
				return (DTO)DslModeling::DomainRoleInfo.GetRolePlayer(this, ChildsDomainRoleId);
			}
			[global::System.Diagnostics.DebuggerStepThrough]
			set
			{
				DslModeling::DomainRoleInfo.SetRolePlayer(this, ChildsDomainRoleId, value);
			}
		}
				
		#endregion
		#region Static methods to access Parent of a DTO
		/// <summary>
		/// Gets Parent.
		/// </summary>
		[global::System.Diagnostics.DebuggerStepThrough]
		[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011")]
		public static DTO GetParent(DTO element)
		{
			return DslModeling::DomainRoleInfo.GetLinkedElement(element, ParentDomainRoleId) as DTO;
		}
		
		/// <summary>
		/// Sets Parent.
		/// </summary>
		[global::System.Diagnostics.DebuggerStepThrough]
		[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011")]
		public static void SetParent(DTO element, DTO newChilds)
		{
			DslModeling::DomainRoleInfo.SetLinkedElement(element, ParentDomainRoleId, newChilds);
		}
		#endregion
		#region Parent domain role code
		
		/// <summary>
		/// Parent domain role Id.
		/// </summary>
		public static readonly global::System.Guid ParentDomainRoleId = new global::System.Guid(0x13da6c52, 0xdf18, 0x4503, 0xad, 0x08, 0x2f, 0x08, 0x05, 0x67, 0xed, 0x0f);
		
		/// <summary>
		/// DomainRole Parent
		/// Description for Andrei15193.DtoMin.DTOReferencesDTOs.Parent
		/// </summary>
		[DslDesign::DisplayNameResource("Andrei15193.DtoMin.DTOReferencesDTOs/Parent.DisplayName", typeof(global::Andrei15193.DtoMin.DtoMinDomainModel), "Andrei15193.DtoMin.GeneratedCode.DomainModelResx")]
		[DslDesign::DescriptionResource("Andrei15193.DtoMin.DTOReferencesDTOs/Parent.Description", typeof(global::Andrei15193.DtoMin.DtoMinDomainModel), "Andrei15193.DtoMin.GeneratedCode.DomainModelResx")]
		[DslModeling::DomainRole(DslModeling::DomainRoleOrder.Target, PropertyName = "Parent", PropertyDisplayNameKey="Andrei15193.DtoMin.DTOReferencesDTOs/Parent.PropertyDisplayName",  PropagatesCopy = DslModeling::PropagatesCopyOption.DoNotPropagateCopy, Multiplicity = DslModeling::Multiplicity.One)]
		[DslModeling::DomainObjectId("13da6c52-df18-4503-ad08-2f080567ed0f")]
		public virtual DTO Parent
		{
			[global::System.Diagnostics.DebuggerStepThrough]
			get
			{
				return (DTO)DslModeling::DomainRoleInfo.GetRolePlayer(this, ParentDomainRoleId);
			}
			[global::System.Diagnostics.DebuggerStepThrough]
			set
			{
				DslModeling::DomainRoleInfo.SetRolePlayer(this, ParentDomainRoleId, value);
			}
		}
				
		#endregion
		#region Static methods to access Childs of a DTO
		/// <summary>
		/// Gets a list of Childs.
		/// </summary>
		[global::System.Diagnostics.DebuggerStepThrough]
		[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011")]
		public static DslModeling::LinkedElementCollection<DTO> GetChilds(DTO element)
		{
			return GetRoleCollection<DslModeling::LinkedElementCollection<DTO>, DTO>(element, ChildsDomainRoleId);
		}
		#endregion
		#region Childs link accessor
		/// <summary>
		/// Get the list of DTOReferencesDTOs links to a DTO.
		/// </summary>
		[global::System.Diagnostics.DebuggerStepThrough]
		[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011")]
		public static global::System.Collections.ObjectModel.ReadOnlyCollection<global::Andrei15193.DtoMin.DTOReferencesDTOs> GetLinksToChilds ( global::Andrei15193.DtoMin.DTO childsInstance )
		{
			return DslModeling::DomainRoleInfo.GetElementLinks<global::Andrei15193.DtoMin.DTOReferencesDTOs>(childsInstance, global::Andrei15193.DtoMin.DTOReferencesDTOs.ChildsDomainRoleId);
		}
		#endregion
		#region Parent link accessor
		/// <summary>
		/// Get the DTOReferencesDTOs link to a DTO.
		/// </summary>
		[global::System.Diagnostics.DebuggerStepThrough]
		[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011")]
		public static global::Andrei15193.DtoMin.DTOReferencesDTOs GetLinkToParent (global::Andrei15193.DtoMin.DTO parentInstance)
		{
			global::System.Collections.Generic.IList<global::Andrei15193.DtoMin.DTOReferencesDTOs> links = DslModeling::DomainRoleInfo.GetElementLinks<global::Andrei15193.DtoMin.DTOReferencesDTOs>(parentInstance, global::Andrei15193.DtoMin.DTOReferencesDTOs.ParentDomainRoleId);
			global::System.Diagnostics.Debug.Assert(links.Count <= 1, "Multiplicity of Parent not obeyed.");
			if ( links.Count == 0 )
			{
				return null;
			}
			else
			{
				return links[0];
			}
		}
		#endregion
		#region DTOReferencesDTOs instance accessors
		
		/// <summary>
		/// Get any DTOReferencesDTOs links between a given DTO and a DTO.
		/// </summary>
		[global::System.Diagnostics.DebuggerStepThrough]
		[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011")]
		public static global::System.Collections.ObjectModel.ReadOnlyCollection<global::Andrei15193.DtoMin.DTOReferencesDTOs> GetLinks( global::Andrei15193.DtoMin.DTO source, global::Andrei15193.DtoMin.DTO target )
		{
			global::System.Collections.Generic.List<global::Andrei15193.DtoMin.DTOReferencesDTOs> outLinks = new global::System.Collections.Generic.List<global::Andrei15193.DtoMin.DTOReferencesDTOs>();
			global::System.Collections.Generic.IList<global::Andrei15193.DtoMin.DTOReferencesDTOs> links = DslModeling::DomainRoleInfo.GetElementLinks<global::Andrei15193.DtoMin.DTOReferencesDTOs>(source, global::Andrei15193.DtoMin.DTOReferencesDTOs.ChildsDomainRoleId);
			foreach ( global::Andrei15193.DtoMin.DTOReferencesDTOs link in links )
			{
				if ( target.Equals(link.Parent) )
				{
					outLinks.Add(link);
				}
			}
			return outLinks.AsReadOnly();
		}
		/// <summary>
		/// Get the one DTOReferencesDTOs link between a given DTOand a DTO.
		/// </summary>
		[global::System.Diagnostics.DebuggerStepThrough]
		[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011")]
		public static global::Andrei15193.DtoMin.DTOReferencesDTOs GetLink( global::Andrei15193.DtoMin.DTO source, global::Andrei15193.DtoMin.DTO target )
		{
			global::System.Collections.Generic.IList<global::Andrei15193.DtoMin.DTOReferencesDTOs> links = DslModeling::DomainRoleInfo.GetElementLinks<global::Andrei15193.DtoMin.DTOReferencesDTOs>(source, global::Andrei15193.DtoMin.DTOReferencesDTOs.ChildsDomainRoleId);
			foreach ( global::Andrei15193.DtoMin.DTOReferencesDTOs link in links )
			{
				if ( target.Equals(link.Parent) )
				{
					return link;
				}
			}
			return null;
		}
		
		#endregion
	}
}
