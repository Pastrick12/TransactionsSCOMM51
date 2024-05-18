CREATE database [TransactionDB]
GO

USE [TransactionDB]
GO
/****** Object:  Table [dbo].[Ventas]    Script Date: 18/05/2024 10:18:06 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ventas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Folio] [int] NOT NULL,
	[Fecha] [datetime] NOT NULL,
	[Cliente] [varchar](250) NOT NULL,
	[Total] [decimal](10, 2) NOT NULL,
 CONSTRAINT [PK_Ventas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VentasDetalle]    Script Date: 18/05/2024 10:18:06 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VentasDetalle](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Renglon] [tinyint] NOT NULL,
	[VentaId] [int] NOT NULL,
	[Cantidad] [decimal](10, 2) NOT NULL,
	[Descripcion] [varchar](150) NOT NULL,
	[PrecioUnitario] [decimal](10, 2) NOT NULL,
	[Importe] [decimal](10, 2) NOT NULL,
 CONSTRAINT [PK_VentasDetalle] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_Ventas_Folio]    Script Date: 18/05/2024 10:18:06 a. m. ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Ventas_Folio] ON [dbo].[Ventas]
(
	[Folio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_VentasDetalle_VentaId_Renglon]    Script Date: 18/05/2024 10:18:06 a. m. ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_VentasDetalle_VentaId_Renglon] ON [dbo].[VentasDetalle]
(
	[VentaId] ASC,
	[Renglon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
