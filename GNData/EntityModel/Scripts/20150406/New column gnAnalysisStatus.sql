alter table gn.GNAnalysisRequests add [LatestRequestPhase] nvarchar(50)  NOT NULL default 'SECONDARY';
alter table gn.GNAnalysisStatus add [AnalysisPhase] nvarchar(50)  NOT NULL DEFAULT 'SECONDARY';