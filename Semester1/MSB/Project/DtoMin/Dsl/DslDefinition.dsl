<?xml version="1.0" encoding="utf-8"?>
<Dsl xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="e6023d52-fdf2-4b84-bcf1-fced385aa11f" Description="Description for Andrei15193.DtoMin.DtoMin" Name="DtoMin" DisplayName="DtoMin" Namespace="Andrei15193.DtoMin" ProductName="DtoMin" CompanyName="Andrei15193" PackageGuid="4f73bb38-772b-4042-a54f-a269413b3951" PackageNamespace="Andrei15193.DtoMin" xmlns="http://schemas.microsoft.com/VisualStudio/2005/DslTools/DslDefinitionModel">
  <Classes>
    <DomainClass Id="41d84654-c7fe-406c-b48a-b9d8821ad7d0" Description="The root in which all other elements are embedded. Appears as a diagram." Name="DTOMap" DisplayName="Data Transfer Object Map" Namespace="Andrei15193.DtoMin">
      <Properties>
        <DomainProperty Id="7d90e7c3-320f-4b91-931c-dea433c807a8" Description="Description for Andrei15193.DtoMin.DTOMap.Namespace" Name="Namespace" DisplayName="Namespace">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Notes>Creates an embedding link when an element is dropped onto a model. </Notes>
          <Index>
            <DomainClassMoniker Name="DTO" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>DTOMapHasDTOs.DTOs</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="b08b385b-b640-4b91-9666-835dbf38ec8b" Description="Elements embedded in the model. Appear as boxes on the diagram." Name="DTO" DisplayName="Data Transfer Object" Namespace="Andrei15193.DtoMin">
      <Properties>
        <DomainProperty Id="5d2e687d-b26f-4fc8-bd4b-01ed24893cb4" Description="Description for Andrei15193.DtoMin.DTO.Name" Name="Name" DisplayName="Name" DefaultValue="" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="DTOAttribute" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>DTOHasAttributes.Attributes</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="8ae96828-4697-43d3-bba7-81a3acb702f9" Description="Description for Andrei15193.DtoMin.DTOAttribute" Name="DTOAttribute" DisplayName="Data Transfer Object Attribute" Namespace="Andrei15193.DtoMin">
      <Properties>
        <DomainProperty Id="a52364c1-af0e-4109-9200-44533d8defc9" Description="Description for Andrei15193.DtoMin.DTOAttribute.Name" Name="Name" DisplayName="Name" DefaultValue="Attribute">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="3b6916d5-02d1-467f-a6ef-53a9c7ab267a" Description="Description for Andrei15193.DtoMin.DTOAttribute.Type" Name="Type" DisplayName="Type">
          <Type>
            <DomainEnumerationMoniker Name="AttributeType" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="cf9375de-d2da-4aee-a377-40031a9de0b0" Description="Description for Andrei15193.DtoMin.DTOAttribute.Multiplicity" Name="Multiplicity" DisplayName="Multiplicity" DefaultValue="Single">
          <Type>
            <DomainEnumerationMoniker Name="Multiplicity" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
  </Classes>
  <Relationships>
    <DomainRelationship Id="73a6cdf0-6cc2-4ac3-9452-ad35ff0441b0" Description="Embedding relationship between the Model and Elements" Name="DTOMapHasDTOs" DisplayName="DTOMap has DTOs" Namespace="Andrei15193.DtoMin" IsEmbedding="true">
      <Source>
        <DomainRole Id="f023c5e1-dc6a-4042-a590-fbbbaca5605c" Description="" Name="DTOs" DisplayName="Data Transfer Objects" PropertyName="DTOs" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="DTOs">
          <RolePlayer>
            <DomainClassMoniker Name="DTOMap" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="061f7842-4217-4050-b70a-13e5b66c9e75" Description="" Name="DTOMap" DisplayName="Data Transfer Object Map" PropertyName="DTOMap" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="DTOMap">
          <RolePlayer>
            <DomainClassMoniker Name="DTO" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="6ab2d098-dad5-4064-b05b-cb6904569e8b" Description="Description for Andrei15193.DtoMin.DTOHasAttributes" Name="DTOHasAttributes" DisplayName="DTO has Attributes" Namespace="Andrei15193.DtoMin" IsEmbedding="true">
      <Source>
        <DomainRole Id="2de75d4d-6c82-40a3-ba7b-354c0c133ee2" Description="Description for Andrei15193.DtoMin.DTOHasAttributes.Attributes" Name="Attributes" DisplayName="Attributes" PropertyName="Attributes" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Attributes">
          <RolePlayer>
            <DomainClassMoniker Name="DTO" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="aaeb625a-d78b-41d8-ac7b-c97af3f9ddde" Description="Description for Andrei15193.DtoMin.DTOHasAttributes.Container" Name="Container" DisplayName="Container" PropertyName="Container" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Container">
          <RolePlayer>
            <DomainClassMoniker Name="DTOAttribute" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="6d3941d9-cf31-4248-b831-2cb4cbc497e2" Description="Description for Andrei15193.DtoMin.DTOReferencesDTOs" Name="DTOReferencesDTOs" DisplayName="DTOReferences DTOs" Namespace="Andrei15193.DtoMin">
      <Source>
        <DomainRole Id="89276f33-27dd-4920-94dc-9a2e96433a61" Description="Description for Andrei15193.DtoMin.DTOReferencesDTOs.Childs" Name="Childs" DisplayName="Childs" PropertyName="Childs" PropertyDisplayName="Childs">
          <RolePlayer>
            <DomainClassMoniker Name="DTO" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="13da6c52-df18-4503-ad08-2f080567ed0f" Description="Description for Andrei15193.DtoMin.DTOReferencesDTOs.Parent" Name="Parent" DisplayName="Parent" PropertyName="Parent" Multiplicity="One" PropertyDisplayName="Parent">
          <RolePlayer>
            <DomainClassMoniker Name="DTO" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
  </Relationships>
  <Types>
    <ExternalType Name="DateTime" Namespace="System" />
    <ExternalType Name="String" Namespace="System" />
    <ExternalType Name="Int16" Namespace="System" />
    <ExternalType Name="Int32" Namespace="System" />
    <ExternalType Name="Int64" Namespace="System" />
    <ExternalType Name="UInt16" Namespace="System" />
    <ExternalType Name="UInt32" Namespace="System" />
    <ExternalType Name="UInt64" Namespace="System" />
    <ExternalType Name="SByte" Namespace="System" />
    <ExternalType Name="Byte" Namespace="System" />
    <ExternalType Name="Double" Namespace="System" />
    <ExternalType Name="Single" Namespace="System" />
    <ExternalType Name="Guid" Namespace="System" />
    <ExternalType Name="Boolean" Namespace="System" />
    <ExternalType Name="Char" Namespace="System" />
    <DomainEnumeration Name="AttributeType" Namespace="Andrei15193.DtoMin" Description="Description for Andrei15193.DtoMin.AttributeType">
      <Literals>
        <EnumerationLiteral Description="Represents a 32-bit integer" Name="Int" Value="1" />
        <EnumerationLiteral Description="Represent a list of characters" Name="Text" Value="0" />
        <EnumerationLiteral Description="Represents a 64-bit floating point number" Name="Float" Value="2" />
        <EnumerationLiteral Description="Represents a date and a time span" Name="DateTime" Value="4" />
      </Literals>
    </DomainEnumeration>
    <DomainEnumeration Name="Multiplicity" Namespace="Andrei15193.DtoMin" Description="Description for Andrei15193.DtoMin.Multiplicity">
      <Literals>
        <EnumerationLiteral Description="Description for Andrei15193.DtoMin.Multiplicity.Single" Name="Single" Value="0" />
        <EnumerationLiteral Description="Description for Andrei15193.DtoMin.Multiplicity.Collection" Name="Collection" Value="1" />
      </Literals>
    </DomainEnumeration>
  </Types>
  <Shapes>
    <CompartmentShape Id="f94c2814-57b8-46c9-b365-d2f9e318726e" Description="Description for Andrei15193.DtoMin.DTOCompartmentShape" Name="DTOCompartmentShape" DisplayName="DTOCompartment Shape" Namespace="Andrei15193.DtoMin" FixedTooltipText="Data Transfer Object" FillColor="LightCoral" InitialHeight="0.3" Geometry="RoundedRectangle">
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="NameTextDecorator" DisplayName="Name Text Decorator" DefaultText="NameTextDecorator" />
      </ShapeHasDecorators>
      <Compartment Name="AttributesCompartment" />
    </CompartmentShape>
  </Shapes>
  <Connectors>
    <Connector Id="0e03d29c-e348-4189-85cb-14dcb4ef9254" Description="Connector between the ExampleShapes. Represents ExampleRelationships on the Diagram." Name="ExampleConnector" DisplayName="Example Connector" Namespace="Andrei15193.DtoMin" FixedTooltipText="Example Connector" Color="113, 111, 110" TargetEndStyle="EmptyArrow" Thickness="0.01" />
  </Connectors>
  <XmlSerializationBehavior Name="DtoMinSerializationBehavior" Namespace="Andrei15193.DtoMin">
    <ClassData>
      <XmlClassData TypeName="DTOMap" MonikerAttributeName="" SerializeId="true" MonikerElementName="dTOMapMoniker" ElementName="dTOMap" MonikerTypeName="DTOMapMoniker">
        <DomainClassMoniker Name="DTOMap" />
        <ElementData>
          <XmlRelationshipData RoleElementName="dTOs">
            <DomainRelationshipMoniker Name="DTOMapHasDTOs" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="namespace">
            <DomainPropertyMoniker Name="DTOMap/Namespace" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="DTO" MonikerAttributeName="name" SerializeId="true" MonikerElementName="dTOMoniker" ElementName="dTO" MonikerTypeName="DTOMoniker">
        <DomainClassMoniker Name="DTO" />
        <ElementData>
          <XmlPropertyData XmlName="name" IsMonikerKey="true">
            <DomainPropertyMoniker Name="DTO/Name" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="attributes">
            <DomainRelationshipMoniker Name="DTOHasAttributes" />
          </XmlRelationshipData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="childs">
            <DomainRelationshipMoniker Name="DTOReferencesDTOs" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="DTOMapHasDTOs" MonikerAttributeName="" SerializeId="true" MonikerElementName="dTOMapHasDTOsMoniker" ElementName="dTOMapHasDTOs" MonikerTypeName="DTOMapHasDTOsMoniker">
        <DomainRelationshipMoniker Name="DTOMapHasDTOs" />
      </XmlClassData>
      <XmlClassData TypeName="ExampleConnector" MonikerAttributeName="" SerializeId="true" MonikerElementName="exampleConnectorMoniker" ElementName="exampleConnector" MonikerTypeName="ExampleConnectorMoniker">
        <ConnectorMoniker Name="ExampleConnector" />
      </XmlClassData>
      <XmlClassData TypeName="DtoMinDiagram" MonikerAttributeName="" SerializeId="true" MonikerElementName="dtoMinDiagramMoniker" ElementName="dtoMinDiagram" MonikerTypeName="DtoMinDiagramMoniker">
        <DiagramMoniker Name="DtoMinDiagram" />
      </XmlClassData>
      <XmlClassData TypeName="DTOCompartmentShape" MonikerAttributeName="" SerializeId="true" MonikerElementName="dTOCompartmentShapeMoniker" ElementName="dTOCompartmentShape" MonikerTypeName="DTOCompartmentShapeMoniker">
        <CompartmentShapeMoniker Name="DTOCompartmentShape" />
      </XmlClassData>
      <XmlClassData TypeName="DTOAttribute" MonikerAttributeName="" SerializeId="true" MonikerElementName="dTOAttributeMoniker" ElementName="dTOAttribute" MonikerTypeName="DTOAttributeMoniker">
        <DomainClassMoniker Name="DTOAttribute" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="DTOAttribute/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="type">
            <DomainPropertyMoniker Name="DTOAttribute/Type" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="multiplicity">
            <DomainPropertyMoniker Name="DTOAttribute/Multiplicity" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="DTOHasAttributes" MonikerAttributeName="" SerializeId="true" MonikerElementName="dTOHasAttributesMoniker" ElementName="dTOHasAttributes" MonikerTypeName="DTOHasAttributesMoniker">
        <DomainRelationshipMoniker Name="DTOHasAttributes" />
      </XmlClassData>
      <XmlClassData TypeName="DTOReferencesDTOs" MonikerAttributeName="" SerializeId="true" MonikerElementName="dTOReferencesDTOsMoniker" ElementName="dTOReferencesDTOs" MonikerTypeName="DTOReferencesDTOsMoniker">
        <DomainRelationshipMoniker Name="DTOReferencesDTOs" />
      </XmlClassData>
    </ClassData>
  </XmlSerializationBehavior>
  <ExplorerBehavior Name="DtoMinExplorer" />
  <ConnectionBuilders>
    <ConnectionBuilder Name="DTOReferencesDTOsBuilder">
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="DTOReferencesDTOs" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="DTO" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="DTO" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
    </ConnectionBuilder>
  </ConnectionBuilders>
  <Diagram Id="9b1605d3-4bff-42ed-91e5-e993b5e510bb" Description="Description for Andrei15193.DtoMin.DtoMinDiagram" Name="DtoMinDiagram" DisplayName="Minimal Language Diagram" Namespace="Andrei15193.DtoMin">
    <Class>
      <DomainClassMoniker Name="DTOMap" />
    </Class>
    <ShapeMaps>
      <CompartmentShapeMap>
        <DomainClassMoniker Name="DTO" />
        <ParentElementPath>
          <DomainPath>DTOMapHasDTOs.DTOMap/!DTOs</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="DTOCompartmentShape/NameTextDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="DTO/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <CompartmentShapeMoniker Name="DTOCompartmentShape" />
        <CompartmentMap>
          <CompartmentMoniker Name="DTOCompartmentShape/AttributesCompartment" />
          <ElementsDisplayed>
            <DomainPath>DTOHasAttributes.Attributes</DomainPath>
          </ElementsDisplayed>
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="DTOAttribute/Name" />
              <DomainPath>DTOHasAttributes!Container</DomainPath>
            </PropertyPath>
          </PropertyDisplayed>
        </CompartmentMap>
      </CompartmentShapeMap>
    </ShapeMaps>
    <ConnectorMaps>
      <ConnectorMap>
        <ConnectorMoniker Name="ExampleConnector" />
        <DomainRelationshipMoniker Name="DTOReferencesDTOs" />
      </ConnectorMap>
    </ConnectorMaps>
  </Diagram>
  <Designer CopyPasteGeneration="CopyPasteOnly" FileExtension="dtom" EditorGuid="f52bbe53-3ad0-47cc-acc9-a2c213b13e54">
    <RootClass>
      <DomainClassMoniker Name="DTOMap" />
    </RootClass>
    <XmlSerializationDefinition CustomPostLoad="false">
      <XmlSerializationBehaviorMoniker Name="DtoMinSerializationBehavior" />
    </XmlSerializationDefinition>
    <ToolboxTab TabText="DtoMin">
      <ElementTool Name="ExampleElement" ToolboxIcon="Resources\ExampleShapeToolBitmap.bmp" Caption="Data Transfer Object" Tooltip="Create a Data Transfer Object" HelpKeyword="CreateExampleClassF1Keyword">
        <DomainClassMoniker Name="DTO" />
      </ElementTool>
      <ConnectionTool Name="DTORelationship" ToolboxIcon="resources\exampleconnectortoolbitmap.bmp" Caption="Data Transfer Object Relation" Tooltip="Drag between Data Transfer Objects to create a Data Transfer Object Relationship" HelpKeyword="ConnectExampleRelationF1Keyword">
        <ConnectionBuilderMoniker Name="DtoMin/DTOReferencesDTOsBuilder" />
      </ConnectionTool>
    </ToolboxTab>
    <Validation UsesMenu="false" UsesOpen="false" UsesSave="false" UsesLoad="false" />
    <DiagramMoniker Name="DtoMinDiagram" />
  </Designer>
  <Explorer ExplorerGuid="25eeeb92-a8f8-4c2d-bdc7-9ec4214ce84a" Title="DtoMin Explorer">
    <ExplorerBehaviorMoniker Name="DtoMin/DtoMinExplorer" />
  </Explorer>
</Dsl>