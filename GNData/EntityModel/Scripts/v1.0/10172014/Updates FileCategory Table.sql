USE [gn_db]
GO

UPDATE GN.GNNotificationTopics SET Subject = RTRIM(Subject), Title = RTRIM(Title);
ALTER TABLE GN.[GNCloudFileCategories] ADD [Type] nvarchar(20)  NOT NULL DEFAULT '*';
ALTER TABLE GN.[GNCloudFileCategories] ADD [FileExtensions] nvarchar(max)  NOT NULL DEFAULT '.';
UPDATE GN.[GNCloudFileCategories] SET [type] = 'OUTPUT', [Name] = 'Variant Call File', [FileExtensions] = '.vcf' WHERE Id = 2;
UPDATE GN.[GNCloudFileCategories] SET [type] = 'INPUT', [FileExtensions] = '.fastq, .fastq.gz, .fastq.zip, .fastq.rar' WHERE Id = 1;
INSERT INTO [gn].[GNCloudFileCategories] ([Name],[FolderPath],[Type],[FileExtensions]) VALUES ('Annotation', 'results', 'OUTPUT', '.table, .xls, .xlsx');
INSERT INTO [gn].[GNCloudFileCategories] ([Name],[FolderPath],[Type],[FileExtensions]) VALUES ('BAM file', 'results', 'OUTPUT', '.bam');
INSERT INTO [gn].[GNCloudFileCategories] ([Name],[FolderPath],[Type],[FileExtensions]) VALUES ('Results file', 'results', 'OUTPUT', '.');


--Update the records of existing annotation files
UPDATE GN.GNCloudFiles
SET GNCloudFileCategoryId = (SELECT id FROM gn.GNCloudFileCategories WHERE [Name] = 'Annotation')
WHERE GNCloudFileCategoryId = 2
AND [Description] NOT LIKE '%.vcf';