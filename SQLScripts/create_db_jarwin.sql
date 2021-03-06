USE [master]
GO

/****** Object:  Database [jarwin]    Script Date: 3/10/2014 7:43:44 a.m. ******/
CREATE DATABASE [jarwin]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'jarwin', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\jarwin.mdf' , SIZE = 22528KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'jarwin_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\jarwin_log.ldf' , SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [jarwin] SET COMPATIBILITY_LEVEL = 120
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [jarwin].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [jarwin] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [jarwin] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [jarwin] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [jarwin] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [jarwin] SET ARITHABORT OFF 
GO

ALTER DATABASE [jarwin] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [jarwin] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [jarwin] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [jarwin] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [jarwin] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [jarwin] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [jarwin] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [jarwin] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [jarwin] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [jarwin] SET  DISABLE_BROKER 
GO

ALTER DATABASE [jarwin] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [jarwin] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [jarwin] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [jarwin] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [jarwin] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [jarwin] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [jarwin] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [jarwin] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [jarwin] SET  MULTI_USER 
GO

ALTER DATABASE [jarwin] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [jarwin] SET DB_CHAINING OFF 
GO

ALTER DATABASE [jarwin] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [jarwin] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO

ALTER DATABASE [jarwin] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [jarwin] SET  READ_WRITE 
GO

