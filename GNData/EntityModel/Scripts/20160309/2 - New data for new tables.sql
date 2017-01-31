
INSERT INTO gn.GNSampleQualifierGroups (Code, Name, CreatedBy, CreateDateTime)
values ('DNA', 'RNASeq', '0750f896-e7d6-48d4-a1b9-007059f62784', getdate());

INSERT INTO gn.GNSampleQualifierGroups (Code, Name, CreatedBy, CreateDateTime)
values ('RNA', 'RNASeq', '0750f896-e7d6-48d4-a1b9-007059f62784', getdate());

INSERT INTO gn.GNSampleQualifiers (Code, Name, GNSampleQualifierGroupCode, CreatedBy, CreateDateTime)
values ('DNA', 'Germline', 'DNA', '0750f896-e7d6-48d4-a1b9-007059f62784', getdate());

INSERT INTO gn.GNSampleQualifiers (Code, Name, GNSampleQualifierGroupCode, CreatedBy, CreateDateTime)
values ('TUMOR', 'Tumor', 'DNA', '0750f896-e7d6-48d4-a1b9-007059f62784', getdate());

INSERT INTO gn.GNSampleQualifiers (Code, Name, GNSampleQualifierGroupCode, CreatedBy, CreateDateTime)
values ('RNA', 'RNA', 'RNA', '0750f896-e7d6-48d4-a1b9-007059f62784', getdate());

select * from gn.GNAnalysisRequests where GNAnalysisRequestTypeCode <> 'DNA'

INSERT INTO gn.GNAnalysisRequestTypes (Code, Name, CreatedBy, CreateDateTime)
values ('DNA', 'DNASeq', '0750f896-e7d6-48d4-a1b9-007059f62784', getdate());

INSERT INTO gn.GNAnalysisRequestTypes (Code, Name, CreatedBy, CreateDateTime)
values ('TUMORNORMAL', 'Tumor-Normal', '0750f896-e7d6-48d4-a1b9-007059f62784', getdate());

INSERT INTO gn.GNAnalysisRequestTypes (Code, Name, CreatedBy, CreateDateTime)
values ('RNA', 'RNASeq', '0750f896-e7d6-48d4-a1b9-007059f62784', getdate());
