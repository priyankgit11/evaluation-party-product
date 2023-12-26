USE [master]
GO
/****** Object:  Database [EvaluationDB]    Script Date: 26-12-2023 15:45:10 ******/
CREATE DATABASE [EvaluationDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'EvaluationDB', FILENAME = N'C:\Users\priya\EvaluationDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'EvaluationDB_log', FILENAME = N'C:\Users\priya\EvaluationDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [EvaluationDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [EvaluationDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [EvaluationDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [EvaluationDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [EvaluationDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [EvaluationDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [EvaluationDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [EvaluationDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [EvaluationDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [EvaluationDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [EvaluationDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [EvaluationDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [EvaluationDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [EvaluationDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [EvaluationDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [EvaluationDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [EvaluationDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [EvaluationDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [EvaluationDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [EvaluationDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [EvaluationDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [EvaluationDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [EvaluationDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [EvaluationDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [EvaluationDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [EvaluationDB] SET  MULTI_USER 
GO
ALTER DATABASE [EvaluationDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [EvaluationDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [EvaluationDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [EvaluationDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [EvaluationDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [EvaluationDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [EvaluationDB] SET QUERY_STORE = OFF
GO
USE [EvaluationDB]
GO
ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = OFF;
GO
USE [EvaluationDB]
GO
/****** Object:  Table [dbo].[tblAssignParty]    Script Date: 26-12-2023 15:45:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblAssignParty](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[party_id] [int] NOT NULL,
	[product_id] [int] NOT NULL,
 CONSTRAINT [PK_tblAssignParty] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblInvoice]    Script Date: 26-12-2023 15:45:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblInvoice](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[party_id] [int] NOT NULL,
	[date] [datetime] NOT NULL,
 CONSTRAINT [PK_tblInvoice] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblInvoiceDetail]    Script Date: 26-12-2023 15:45:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblInvoiceDetail](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[invoice_id] [int] NOT NULL,
	[product_id] [int] NOT NULL,
	[rate] [decimal](9, 2) NOT NULL,
	[quantity] [int] NOT NULL,
 CONSTRAINT [PK_tblInvoiceDetail] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblParty]    Script Date: 26-12-2023 15:45:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblParty](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[party_name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_tblParty] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblProduct]    Script Date: 26-12-2023 15:45:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblProduct](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[product_name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_tblProduct] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblProductRate]    Script Date: 26-12-2023 15:45:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblProductRate](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[product_id] [int] NOT NULL,
	[rate] [decimal](9, 2) NOT NULL,
 CONSTRAINT [PK_tblProductRate] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tblAssignParty]  WITH CHECK ADD  CONSTRAINT [FK_tblAssignParty_tblParty] FOREIGN KEY([party_id])
REFERENCES [dbo].[tblParty] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tblAssignParty] CHECK CONSTRAINT [FK_tblAssignParty_tblParty]
GO
ALTER TABLE [dbo].[tblAssignParty]  WITH CHECK ADD  CONSTRAINT [FK_tblAssignParty_tblProduct] FOREIGN KEY([product_id])
REFERENCES [dbo].[tblProduct] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tblAssignParty] CHECK CONSTRAINT [FK_tblAssignParty_tblProduct]
GO
ALTER TABLE [dbo].[tblInvoice]  WITH CHECK ADD  CONSTRAINT [FK_tblInvoice_tblParty] FOREIGN KEY([party_id])
REFERENCES [dbo].[tblParty] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tblInvoice] CHECK CONSTRAINT [FK_tblInvoice_tblParty]
GO
ALTER TABLE [dbo].[tblInvoiceDetail]  WITH CHECK ADD  CONSTRAINT [FK_tblInvoiceDetail_tblInvoice] FOREIGN KEY([invoice_id])
REFERENCES [dbo].[tblInvoice] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tblInvoiceDetail] CHECK CONSTRAINT [FK_tblInvoiceDetail_tblInvoice]
GO
ALTER TABLE [dbo].[tblInvoiceDetail]  WITH CHECK ADD  CONSTRAINT [FK_tblInvoiceDetail_tblProduct] FOREIGN KEY([product_id])
REFERENCES [dbo].[tblProduct] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tblInvoiceDetail] CHECK CONSTRAINT [FK_tblInvoiceDetail_tblProduct]
GO
ALTER TABLE [dbo].[tblProductRate]  WITH CHECK ADD  CONSTRAINT [FK_tblProductRate_tblProduct] FOREIGN KEY([product_id])
REFERENCES [dbo].[tblProduct] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tblProductRate] CHECK CONSTRAINT [FK_tblProductRate_tblProduct]
GO
USE [master]
GO
ALTER DATABASE [EvaluationDB] SET  READ_WRITE 
GO
