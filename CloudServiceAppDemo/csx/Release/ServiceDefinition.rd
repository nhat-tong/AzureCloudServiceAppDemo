<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="CloudServiceAppDemo" generation="1" functional="0" release="0" Id="86c7dd13-9e6d-4525-b1a4-a466d53197c6" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="CloudServiceAppDemoGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="AdsWeb:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/CloudServiceAppDemo/CloudServiceAppDemoGroup/LB:AdsWeb:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="AdsWeb:APPINSIGHTS_INSTRUMENTATIONKEY" defaultValue="">
          <maps>
            <mapMoniker name="/CloudServiceAppDemo/CloudServiceAppDemoGroup/MapAdsWeb:APPINSIGHTS_INSTRUMENTATIONKEY" />
          </maps>
        </aCS>
        <aCS name="AdsWeb:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/CloudServiceAppDemo/CloudServiceAppDemoGroup/MapAdsWeb:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="AdsWeb:StorageConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/CloudServiceAppDemo/CloudServiceAppDemoGroup/MapAdsWeb:StorageConnectionString" />
          </maps>
        </aCS>
        <aCS name="AdsWebInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/CloudServiceAppDemo/CloudServiceAppDemoGroup/MapAdsWebInstances" />
          </maps>
        </aCS>
        <aCS name="AdsWorker:APPINSIGHTS_INSTRUMENTATIONKEY" defaultValue="">
          <maps>
            <mapMoniker name="/CloudServiceAppDemo/CloudServiceAppDemoGroup/MapAdsWorker:APPINSIGHTS_INSTRUMENTATIONKEY" />
          </maps>
        </aCS>
        <aCS name="AdsWorker:ContosoAdsDbConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/CloudServiceAppDemo/CloudServiceAppDemoGroup/MapAdsWorker:ContosoAdsDbConnectionString" />
          </maps>
        </aCS>
        <aCS name="AdsWorker:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/CloudServiceAppDemo/CloudServiceAppDemoGroup/MapAdsWorker:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="AdsWorker:StorageConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/CloudServiceAppDemo/CloudServiceAppDemoGroup/MapAdsWorker:StorageConnectionString" />
          </maps>
        </aCS>
        <aCS name="AdsWorkerInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/CloudServiceAppDemo/CloudServiceAppDemoGroup/MapAdsWorkerInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:AdsWeb:Endpoint1">
          <toPorts>
            <inPortMoniker name="/CloudServiceAppDemo/CloudServiceAppDemoGroup/AdsWeb/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapAdsWeb:APPINSIGHTS_INSTRUMENTATIONKEY" kind="Identity">
          <setting>
            <aCSMoniker name="/CloudServiceAppDemo/CloudServiceAppDemoGroup/AdsWeb/APPINSIGHTS_INSTRUMENTATIONKEY" />
          </setting>
        </map>
        <map name="MapAdsWeb:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/CloudServiceAppDemo/CloudServiceAppDemoGroup/AdsWeb/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapAdsWeb:StorageConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/CloudServiceAppDemo/CloudServiceAppDemoGroup/AdsWeb/StorageConnectionString" />
          </setting>
        </map>
        <map name="MapAdsWebInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/CloudServiceAppDemo/CloudServiceAppDemoGroup/AdsWebInstances" />
          </setting>
        </map>
        <map name="MapAdsWorker:APPINSIGHTS_INSTRUMENTATIONKEY" kind="Identity">
          <setting>
            <aCSMoniker name="/CloudServiceAppDemo/CloudServiceAppDemoGroup/AdsWorker/APPINSIGHTS_INSTRUMENTATIONKEY" />
          </setting>
        </map>
        <map name="MapAdsWorker:ContosoAdsDbConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/CloudServiceAppDemo/CloudServiceAppDemoGroup/AdsWorker/ContosoAdsDbConnectionString" />
          </setting>
        </map>
        <map name="MapAdsWorker:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/CloudServiceAppDemo/CloudServiceAppDemoGroup/AdsWorker/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapAdsWorker:StorageConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/CloudServiceAppDemo/CloudServiceAppDemoGroup/AdsWorker/StorageConnectionString" />
          </setting>
        </map>
        <map name="MapAdsWorkerInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/CloudServiceAppDemo/CloudServiceAppDemoGroup/AdsWorkerInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="AdsWeb" generation="1" functional="0" release="0" software="D:\NHAT\MVC PROJECTS\Exercices\1. Design_App_Archi\3. Azure_Roles\AzureCloudServiceApp\CloudServiceAppDemo\csx\Release\roles\AdsWeb" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="-1" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="APPINSIGHTS_INSTRUMENTATIONKEY" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="StorageConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;AdsWeb&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;AdsWeb&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;AdsWorker&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/CloudServiceAppDemo/CloudServiceAppDemoGroup/AdsWebInstances" />
            <sCSPolicyUpdateDomainMoniker name="/CloudServiceAppDemo/CloudServiceAppDemoGroup/AdsWebUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/CloudServiceAppDemo/CloudServiceAppDemoGroup/AdsWebFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="AdsWorker" generation="1" functional="0" release="0" software="D:\NHAT\MVC PROJECTS\Exercices\1. Design_App_Archi\3. Azure_Roles\AzureCloudServiceApp\CloudServiceAppDemo\csx\Release\roles\AdsWorker" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="-1" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="APPINSIGHTS_INSTRUMENTATIONKEY" defaultValue="" />
              <aCS name="ContosoAdsDbConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="StorageConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;AdsWorker&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;AdsWeb&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;AdsWorker&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/CloudServiceAppDemo/CloudServiceAppDemoGroup/AdsWorkerInstances" />
            <sCSPolicyUpdateDomainMoniker name="/CloudServiceAppDemo/CloudServiceAppDemoGroup/AdsWorkerUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/CloudServiceAppDemo/CloudServiceAppDemoGroup/AdsWorkerFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="AdsWebUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyUpdateDomain name="AdsWorkerUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="AdsWebFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="AdsWorkerFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="AdsWebInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="AdsWorkerInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="844d25aa-8b8c-4041-8b54-bd3b2331afea" ref="Microsoft.RedDog.Contract\ServiceContract\CloudServiceAppDemoContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="a68dee74-d3d1-47ac-8dfe-f04af9f4a837" ref="Microsoft.RedDog.Contract\Interface\AdsWeb:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/CloudServiceAppDemo/CloudServiceAppDemoGroup/AdsWeb:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>