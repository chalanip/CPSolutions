#﻿CREATE SCHEMA `chalanitest` ;

CREATE TABLE `users` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `UserNumber` VARCHAR(20) NOT NULL,
  `IsActive` BIT NOT NULL,
  PRIMARY KEY (`Id`));
  
CREATE TABLE `subscriptionhistory` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `UserId` INT NOT NULL,
  `Description` VARCHAR(100) NOT NULL,
  `DateTimeStamp` DateTime NOT NULL,
  PRIMARY KEY (`Id`));
  

CREATE TABLE `usercouples` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `UserId` INT NOT NULL,
  `CoupleUserId` INT NOT NULL,
  `Code` VARCHAR(20) NOT NULL,
  PRIMARY KEY (`Id`));

CREATE TABLE `activitylog` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `UserId` INT NOT NULL,
  `Description` VARCHAR(100) NOT NULL,
  `DateTimeStamp` DateTime NOT NULL,
  PRIMARY KEY (`Id`));
  
  
CREATE TABLE `charginghistory` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `UserId` INT NOT NULL,
  `LastPaymentDateTime` DateTime NOT NULL,
  PRIMARY KEY (`Id`));