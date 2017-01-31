
alter table gn.GNContacts add [IsSubscribedForNewsletters] bit NOT NULL default 1;

update gn.GNContacts set IsSubscribedForNewsletters = 0 where IsInviteAccepted = 0;

