USE [master]
GO
/****** Object:  Database [OrderManagementSystem]    Script Date: 9/14/2019 8:14:19 AM ******/
CREATE DATABASE [OrderManagementSystem]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'OrderManagementSystem', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER01\MSSQL\DATA\OrderManagementSystem.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'OrderManagementSystem_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER01\MSSQL\DATA\OrderManagementSystem_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [OrderManagementSystem] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [OrderManagementSystem].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [OrderManagementSystem] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [OrderManagementSystem] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [OrderManagementSystem] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [OrderManagementSystem] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [OrderManagementSystem] SET ARITHABORT OFF 
GO
ALTER DATABASE [OrderManagementSystem] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [OrderManagementSystem] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [OrderManagementSystem] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [OrderManagementSystem] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [OrderManagementSystem] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [OrderManagementSystem] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [OrderManagementSystem] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [OrderManagementSystem] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [OrderManagementSystem] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [OrderManagementSystem] SET  DISABLE_BROKER 
GO
ALTER DATABASE [OrderManagementSystem] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [OrderManagementSystem] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [OrderManagementSystem] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [OrderManagementSystem] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [OrderManagementSystem] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [OrderManagementSystem] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [OrderManagementSystem] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [OrderManagementSystem] SET RECOVERY FULL 
GO
ALTER DATABASE [OrderManagementSystem] SET  MULTI_USER 
GO
ALTER DATABASE [OrderManagementSystem] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [OrderManagementSystem] SET DB_CHAINING OFF 
GO
ALTER DATABASE [OrderManagementSystem] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [OrderManagementSystem] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [OrderManagementSystem] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'OrderManagementSystem', N'ON'
GO
ALTER DATABASE [OrderManagementSystem] SET QUERY_STORE = OFF
GO
USE [OrderManagementSystem]
GO
/****** Object:  Table [dbo].[AppAdministrator]    Script Date: 9/14/2019 8:14:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppAdministrator](
	[AdimID] [int] IDENTITY(1,1) NOT NULL,
	[AdminName] [varchar](max) NULL,
	[AdminEmail] [varchar](max) NULL,
	[AdminPassword] [varchar](max) NULL,
 CONSTRAINT [PK_AppAdministrator] PRIMARY KEY CLUSTERED 
(
	[AdimID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Buyers]    Script Date: 9/14/2019 8:14:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Buyers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BuyerID] [varchar](max) NULL,
	[BuyerName] [varchar](max) NULL,
	[Email] [varchar](max) NULL,
	[Password] [varchar](max) NULL,
	[ShippingAddress] [varchar](max) NULL,
 CONSTRAINT [PK_Buyers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Table_OrderDetails]    Script Date: 9/14/2019 8:14:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Table_OrderDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [varchar](max) NULL,
	[BuyerID] [varchar](max) NULL,
	[BuyerEmail] [varchar](max) NULL,
	[OrderItemID] [varchar](max) NULL,
	[ProductID] [varchar](max) NULL,
	[OrderQuantity] [int] NULL,
	[OrderStatus] [varchar](max) NULL,
 CONSTRAINT [PK_Table_OrderDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Table_ProductImage]    Script Date: 9/14/2019 8:14:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Table_ProductImage](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [varchar](max) NULL,
	[ProductImage] [varchar](max) NULL,
 CONSTRAINT [PK_Table_ProductImage] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Table_ProductsDetails]    Script Date: 9/14/2019 8:14:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Table_ProductsDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [varchar](max) NULL,
	[ProductName] [varchar](max) NULL,
	[ProductWeight] [varchar](max) NULL,
	[ProductHeight] [varchar](max) NULL,
	[SKU] [int] NULL,
	[Barcode] [varchar](max) NULL,
	[AvailableQuantity] [int] NULL,
 CONSTRAINT [PK_Table_ProductsDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[AppAdministrator] ON 

INSERT [dbo].[AppAdministrator] ([AdimID], [AdminName], [AdminEmail], [AdminPassword]) VALUES (1, N'Peter Stark', N'pstark@gmail.com', N'pstark30071993')
SET IDENTITY_INSERT [dbo].[AppAdministrator] OFF
SET IDENTITY_INSERT [dbo].[Buyers] ON 

INSERT [dbo].[Buyers] ([ID], [BuyerID], [BuyerName], [Email], [Password], [ShippingAddress]) VALUES (1, N'B1', N'Speed', N'speedprince77967@gmail.com', N'3071993', N'Z/3 299 RSR Kol-70055')
INSERT [dbo].[Buyers] ([ID], [BuyerID], [BuyerName], [Email], [Password], [ShippingAddress]) VALUES (2, N'B2', N'Aoryan', N'aoryan77@gmail.com', N'3071993', N'Z/3 299 RSR Kol-70055')
INSERT [dbo].[Buyers] ([ID], [BuyerID], [BuyerName], [Email], [Password], [ShippingAddress]) VALUES (3, N'B3', N'Sumit', N'bmctechnology.pvt@gmail.com', N'3071993', N'22/9 Sd Road Kol-70065')
SET IDENTITY_INSERT [dbo].[Buyers] OFF
SET IDENTITY_INSERT [dbo].[Table_OrderDetails] ON 

INSERT [dbo].[Table_OrderDetails] ([ID], [OrderID], [BuyerID], [BuyerEmail], [OrderItemID], [ProductID], [OrderQuantity], [OrderStatus]) VALUES (2, N'ODB1', N'B1', N'speedprince77967@gmail.com', N'ODB1P2', N'P2', 5, N'Approved')
INSERT [dbo].[Table_OrderDetails] ([ID], [OrderID], [BuyerID], [BuyerEmail], [OrderItemID], [ProductID], [OrderQuantity], [OrderStatus]) VALUES (3, N'ODB2', N'B2', N'aoryan77@gmail.com', N'ODB2P2', N'P2', 5, N'Placed')
SET IDENTITY_INSERT [dbo].[Table_OrderDetails] OFF
SET IDENTITY_INSERT [dbo].[Table_ProductImage] ON 

INSERT [dbo].[Table_ProductImage] ([ID], [ProductID], [ProductImage]) VALUES (1, N'P1', N'~/ProductImages/mouseP1.jpg')
INSERT [dbo].[Table_ProductImage] ([ID], [ProductID], [ProductImage]) VALUES (2, N'P2', N'~/ProductImages/joystckP2.jpg')
SET IDENTITY_INSERT [dbo].[Table_ProductImage] OFF
SET IDENTITY_INSERT [dbo].[Table_ProductsDetails] ON 

INSERT [dbo].[Table_ProductsDetails] ([ID], [ProductID], [ProductName], [ProductWeight], [ProductHeight], [SKU], [Barcode], [AvailableQuantity]) VALUES (1, N'P1', N'Mouse', N'0.2 kg', N'1 inch', 70, N'235689', 70)
INSERT [dbo].[Table_ProductsDetails] ([ID], [ProductID], [ProductName], [ProductWeight], [ProductHeight], [SKU], [Barcode], [AvailableQuantity]) VALUES (2, N'P2', N'Joystick', N'0.35 kg', N'2.5 inch', 77, N'235689', 67)
SET IDENTITY_INSERT [dbo].[Table_ProductsDetails] OFF
/****** Object:  StoredProcedure [dbo].[JoinOrderDetails]    Script Date: 9/14/2019 8:14:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[JoinOrderDetails] 
	-- Add the parameters for the stored procedure here
@BuyerID Varchar(max) = null	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select BuyerName, ShippingAddress, OrderID, BuyerEmail, OrderItemID, OrderQuantity, OrderStatus, ProductImage, ProductName, ProductWeight, ProductHeight
	from Buyers bu inner join Table_OrderDetails od on bu.BuyerID = od.BuyerID inner join Table_ProductsDetails pd on od.ProductID = pd.ProductID
	inner join Table_ProductImage pi on pd.ProductID = pi.ProductID
	Where od.BuyerID = @BuyerID
END
GO
/****** Object:  StoredProcedure [dbo].[JoinOrderDetailsForAdmin]    Script Date: 9/14/2019 8:14:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[JoinOrderDetailsForAdmin] 
	-- Add the parameters for the stored procedure here	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select BuyerName, ShippingAddress, OrderID, BuyerEmail, OrderItemID, OrderQuantity, OrderStatus, ProductImage, ProductName, ProductWeight, ProductHeight
	from Buyers bu inner join Table_OrderDetails od on bu.BuyerID = od.BuyerID inner join Table_ProductsDetails pd on od.ProductID = pd.ProductID
	inner join Table_ProductImage pi on pd.ProductID = pi.ProductID
END
GO
USE [master]
GO
ALTER DATABASE [OrderManagementSystem] SET  READ_WRITE 
GO
