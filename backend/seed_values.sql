USE [PizzaPicker]
GO


-------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------


/****** Object:  Table [dbo].[Sauce]    Script Date: 1/26/2020 10:34:43 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Sauce](
	[Id] [nvarchar](128) NOT NULL DEFAULT (newid()),
	[Name] [nvarchar](30) NOT NULL,
	[Version] [timestamp] NOT NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL DEFAULT (sysutcdatetime()),
	[UpdatedAt] [datetimeoffset](7) NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_Sauce] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

INSERT [dbo].[Sauce] ([Name], deleted) VALUES (N'Alfredo Sauce', 0)
GO
INSERT [dbo].[Sauce] ([Name], deleted) VALUES ( N'BBQ Sauce', 0)
GO
INSERT [dbo].[Sauce] ([Name], deleted) VALUES (N'Garlic Parmesan White Sauce', 0)
GO
INSERT [dbo].[Sauce] ([Name], deleted) VALUES ( N'Hearty Marinara Sauce', 0)
GO
INSERT [dbo].[Sauce] ([Name], deleted) VALUES (N'Robust Inspired Tomato Sauce', 0)
GO
select * from sauce


-------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------



/****** Object:  Table [dbo].[Topping]    Script Date: 1/26/2020 10:37:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Topping](
	[Id] [nvarchar](128) NOT NULL DEFAULT (newid()),
	[Name] [nvarchar](30) NOT NULL,
	[IsMeat] [bit] NOT NULL,
	[IsCheese] [bit] NOT NULL,
	[Version] [timestamp] NOT NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL DEFAULT (sysutcdatetime()),
	[UpdatedAt] [datetimeoffset](7) NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_Topping] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

INSERT [dbo].[Topping] ([Name], [IsMeat], [Ischeese], deleted) VALUES (N'Bacon', 1, 0, 0)
GO
INSERT [dbo].[Topping] ([Name], [IsMeat], [Ischeese], deleted) VALUES (N'Banana Peppers', 0, 0, 0)
GO
INSERT [dbo].[Topping] ([Name], [IsMeat], [Ischeese], deleted) VALUES (N'Beef', 1, 0, 0)
GO
INSERT [dbo].[Topping] ([Name], [IsMeat], [Ischeese], deleted) VALUES (N'Black Olives', 0, 0, 0)
GO
INSERT [dbo].[Topping] ([Name], [IsMeat], [Ischeese], deleted) VALUES (N'Cheddar Cheese', 0, 1, 0)
GO
INSERT [dbo].[Topping] ([Name], [IsMeat], [Ischeese], deleted) VALUES (N'Diced Tomatoes', 0, 0, 0)
GO
INSERT [dbo].[Topping] ([Name], [IsMeat], [Ischeese], deleted) VALUES (N'Feta Cheese', 0, 1, 0)
GO
INSERT [dbo].[Topping] ([Name], [IsMeat], [Ischeese], deleted) VALUES (N'Green Olives', 0, 0, 0)
GO
INSERT [dbo].[Topping] ([Name], [IsMeat], [Ischeese], deleted) VALUES (N'Green Peppers', 0, 0, 0)
GO
INSERT [dbo].[Topping] ([Name], [IsMeat], [Ischeese], deleted) VALUES (N'Ham', 1, 0, 0)
GO
INSERT [dbo].[Topping] ([Name], [IsMeat], [Ischeese], deleted) VALUES (N'Hot Sauce', 0, 0, 0)
GO
INSERT [dbo].[Topping] ([Name], [IsMeat], [Ischeese], deleted) VALUES (N'Italian Sausage', 1, 0, 0)
GO
INSERT [dbo].[Topping] ([Name], [IsMeat], [Ischeese], deleted) VALUES (N'Jalapeno Peppers', 0, 0, 0)
GO
INSERT [dbo].[Topping] ([Name], [IsMeat], [Ischeese], deleted) VALUES (N'Mushrooms', 0, 0, 0)
GO
INSERT [dbo].[Topping] ([Name], [IsMeat], [Ischeese], deleted) VALUES (N'Onion', 0, 0, 0)
GO
INSERT [dbo].[Topping] ([Name], [IsMeat], [Ischeese], deleted) VALUES (N'Pepperoni', 1, 0, 0)
GO
INSERT [dbo].[Topping] ([Name], [IsMeat], [Ischeese], deleted) VALUES (N'Philly Steak', 1, 0, 0)
GO
INSERT [dbo].[Topping] ([Name], [IsMeat], [Ischeese], deleted) VALUES (N'Pineapple', 0, 0, 0)
GO
INSERT [dbo].[Topping] ([Name], [IsMeat], [Ischeese], deleted) VALUES (N'Premium Chicken', 1, 0, 0)
GO
INSERT [dbo].[Topping] ([Name], [IsMeat], [Ischeese], deleted) VALUES (N'Roasted Red Peppers', 0, 0, 0)
GO
INSERT [dbo].[Topping] ([Name], [IsMeat], [Ischeese], deleted) VALUES (N'Salami', 1, 0, 0)
GO
INSERT [dbo].[Topping] ([Name], [IsMeat], [Ischeese], deleted) VALUES (N'Shredded Parmesan Asiago', 0, 1, 0)
GO
INSERT [dbo].[Topping] ([Name], [IsMeat], [Ischeese], deleted) VALUES (N'Shredded Provolone Cheese', 0, 1, 0)
GO
INSERT [dbo].[Topping] ([Name], [IsMeat], [Ischeese], deleted) VALUES (N'Spinach', 0, 0, 0)
GO

select * from topping




-------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------


/****** Object:  Table [dbo].[Pizza]    Script Date: 1/26/2020 10:53:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Pizza](
	[Id] [nvarchar](128) NOT NULL,
	[SauceId] [nvarchar](128) NOT NULL,
	[Topping1Id] [nvarchar](128) NOT NULL,
	[Topping2Id] [nvarchar](128) NOT NULL,
	[Topping3Id] [nvarchar](128) NOT NULL,
	[Eaten] [bit] NOT NULL,
	[DateEaten] [datetime] NOT NULL,
	[Rating] [decimal](4, 2) NOT NULL,
	[Comment] [nvarchar](max) NULL,
	[Version] [timestamp] NOT NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL,
	[UpdatedAt] [datetimeoffset](7) NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_Pizza] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_Pizza_Toppings] UNIQUE NONCLUSTERED 
(
	[Topping1Id] ASC,
	[Topping2Id] ASC,
	[Topping3Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[Pizza] ADD  DEFAULT (newid()) FOR [Id]
GO

ALTER TABLE [dbo].[Pizza] ADD  CONSTRAINT [DF_Pizza_DateEaten]  DEFAULT ('1900-01-01') FOR [DateEaten]
GO

ALTER TABLE [dbo].[Pizza] ADD  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO

ALTER TABLE [dbo].[Pizza]  WITH CHECK ADD  CONSTRAINT [FK_Pizza_Sauce] FOREIGN KEY([SauceId])
REFERENCES [dbo].[Sauce] ([Id])
GO

ALTER TABLE [dbo].[Pizza] CHECK CONSTRAINT [FK_Pizza_Sauce]
GO

ALTER TABLE [dbo].[Pizza]  WITH CHECK ADD  CONSTRAINT [FK_Pizza_Topping1] FOREIGN KEY([Topping1Id])
REFERENCES [dbo].[Topping] ([Id])
GO

ALTER TABLE [dbo].[Pizza] CHECK CONSTRAINT [FK_Pizza_Topping1]
GO

ALTER TABLE [dbo].[Pizza]  WITH CHECK ADD  CONSTRAINT [FK_Pizza_Topping2] FOREIGN KEY([Topping2Id])
REFERENCES [dbo].[Topping] ([Id])
GO

ALTER TABLE [dbo].[Pizza] CHECK CONSTRAINT [FK_Pizza_Topping2]
GO

ALTER TABLE [dbo].[Pizza]  WITH CHECK ADD  CONSTRAINT [FK_Pizza_Topping3] FOREIGN KEY([Topping3Id])
REFERENCES [dbo].[Topping] ([Id])
GO

ALTER TABLE [dbo].[Pizza] CHECK CONSTRAINT [FK_Pizza_Topping3]
GO

ALTER TABLE [dbo].[Pizza]  WITH CHECK ADD  CONSTRAINT [CK_Pizza_Check_Toppings] CHECK  (([Topping1Id]<>[Topping2Id] AND [Topping1Id]<>[Topping3Id] AND [Topping2Id]<>[Topping3Id]))
GO

ALTER TABLE [dbo].[Pizza] CHECK CONSTRAINT [CK_Pizza_Check_Toppings]
GO
