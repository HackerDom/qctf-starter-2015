/****** users ******/

create table [dbo].[users]
(
	[login] [nvarchar](64) not null,
	[pass] [nvarchar](64) not null,
	[avatar] [nvarchar](256) null
)

create unique nonclustered index [ix_pass] on [dbo].[users] 
(
	[pass] asc
)

create unique nonclustered index [ix_users] on [dbo].[users] 
(
	[login] asc
)


/****** flags ******/

create table [dbo].[flags]
(
	[login] [nvarchar](64) not null,
	[flag] [nvarchar](64) not null,
	[type] [nvarchar](16) not null,
	[dt] [datetime2](7) not null
)

create nonclustered index [ix_flags] on [dbo].[flags] 
(
	[login] asc
)

create unique nonclustered index [ix_uniq] on [dbo].[flags] 
(
	[login] asc,
	[flag] asc
) with (ignore_dup_key = on)


/****** files ******/

create table [dbo].[files]
(
	[login] [nvarchar](64) not null,
	[name] [nvarchar](256) not null,
	[ext] [nvarchar](256) null,
	[url] [nvarchar](256) not null,
	[dt] [datetime2](7) not null
)

create nonclustered index [ix_files] on [dbo].[files] 
(
	[login] asc
)

create unique nonclustered index [ix_uniq] on [dbo].[files] 
(
	[login] asc,
	[url] asc
) with (ignore_dup_key = on)


/****** chat ******/

create table [dbo].[chat]
(
	[id] [bigint] identity(1,1) not null,
	[dt] [datetime2](7) not null,
	[type] [nvarchar](16) not null,
	[login] [nvarchar](64) not null,
	[text] [nvarchar](4000) not null
)

create nonclustered index [ix_login] on [dbo].[chat] 
(
	[login] asc
)


/****** broadcast ******/

create table [dbo].[broadcast]
(
	[login] [nvarchar](64) not null,
	[revision] [bigint] not null,
	[dt] [datetime2](7) not null
)

create nonclustered index [ix_broadcast] on [dbo].[broadcast] 
(
	[login] asc
)

go
