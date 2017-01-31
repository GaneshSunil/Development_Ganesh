
alter table dbo.AspNetRoles add HierarchyOrder int null;

update dbo.AspNetRoles set HierarchyOrder = 1 where [Name] = N'GN_ADMIN';
update dbo.AspNetRoles set HierarchyOrder = 2 where [Name] LIKE N'GN_%';
update dbo.AspNetRoles set HierarchyOrder = 3 where [Name] = N'ORG_MANAGER';
update dbo.AspNetRoles set HierarchyOrder = 4 where [Name] = N'TEAM_MANAGER';
update dbo.AspNetRoles set HierarchyOrder = 5 where [Name] = N'PROJECT_MANAGER';
update dbo.AspNetRoles set HierarchyOrder = 6 where [Name] = N'TEAM_MEMBER';
update dbo.AspNetRoles set HierarchyOrder = 99 where HierarchyOrder is null;
