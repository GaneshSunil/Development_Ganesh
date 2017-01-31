update gn.GNCloudFiles set GNCloudFileCategoryId = 2 where GNCloudFileCategoryId = 6 and FileName like '%vcf'
update gn.GNCloudFiles set GNCloudFileCategoryId = 3 where GNCloudFileCategoryId = 6 and (FileName like '%table%' or FileName like '%xls%')
update gn.GNCloudFiles set GNCloudFileCategoryId = 4 where GNCloudFileCategoryId = 6 and (FileName like '%bam%' or FileName like '%bai%')

