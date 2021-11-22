#!/bin/bash

# How to
# To run this bash script, sign into the tenant where the service principal have been created using: az login.

#############
# CONSTANTS #
#############

#app Id of the multi-tenant Azure AD app registration
appId="0fa4918a-446d-438b-af46-56057b03eed6"

# The application role that you want to content to on this tenant.
graphApplicationPermission="User.Read.All"

# The resource id of Microsoft Graph.
graphClientId="00000003-0000-0000-c000-000000000000"

#####################
# Collect variables #
#####################

# The object id of the service principal
principalId="$( az ad sp show --id $appId --query "objectId" -o tsv )"

# The object id for the Graph Resource in the tenant 
resourceId="$( az ad sp show --id "$graphClientId" --query "objectId" -o tsv )"

# The application role id for the permission
permissionId="$( az ad sp show --id $graphClientId --query "appRoles[?value=='$graphApplicationPermission'].id" -o tsv )"

echo "Service Principal Object Id:                            $principalId"
echo "Object Id of the Microsoft Graph API Service Principal: $resourceId"
echo "Permission Id (id for Role):                            $permissionId"

###########################
# Execute role assignment #
###########################

# app Id of the multi-tenant Azure AD app registrationApplicationPermission)

az rest --method POST --uri https://graph.microsoft.com/beta/servicePrincipals/$resourceId/appRoleAssignments \
        --header Content-Type=application/json \
        --body "{
          \"principalId\": \"$principalId\",
          \"resourceId\": \"$resourceId\",
          \"appRoleId\": \"$permissionId\"
        }"