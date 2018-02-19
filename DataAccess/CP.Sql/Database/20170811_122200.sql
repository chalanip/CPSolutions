CREATE TABLE `chalani`.`categories` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(45) NOT NULL,
  `IsActive` BIT NOT NULL,
  `Description` VARCHAR(200) NULL,
  PRIMARY KEY (`Id`));
  
  CREATE TABLE `chalani`.`users` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(100) NOT NULL,
  `NIC` VARCHAR(10) NULL,
  `ContactNo` VARCHAR(20) NULL,
  `IsActive` BIT NOT NULL,
  `Town` VARCHAR(100) NULL,
  `CategoryId` INT NULL,
  PRIMARY KEY (`Id`));
  
INSERT INTO `chalani`.`categories` (`Name`, `IsActive`, `Description`) VALUES ('HouseMaid', 1, 'Do all house work');
INSERT INTO `chalani`.`categories` (`Name`, `IsActive`, `Description`) VALUES ('Nanny', 1, 'Take care of kids');
INSERT INTO `chalani`.`categories` (`Name`, `IsActive`, `Description`) VALUES ('Driver', 1, 'Drive the vehicle');

