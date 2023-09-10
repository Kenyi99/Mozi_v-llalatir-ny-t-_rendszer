CREATE DATABASE IF NOT EXISTS kenyioffice
CHARACTER set utf8 
COLLATE utf8_hungarian_ci;
USE kenyioffice;

CREATE TABLE IF NOT EXISTS `login` (
              `UserID` INT(11) AUTO_INCREMENT, 
              `UserName` VARCHAR(50) NOT NULL, 
              `Password` LONGTEXT NOT NULL,
              `Permission` INT(11) UNSIGNED NOT NULL, 
              `Email` VARCHAR(255) NOT NULL, 
              PRIMARY KEY(UserID));

insert into login (`UserName`, `Password`, `Permission`, `Email`) Select 'admin', md5('admin'), '1', 'admin@admin.com' Where not exists(select * from login where UserName='admin');

CREATE TABLE IF NOT EXISTS `jobs`(
              `job_id` INT(11) AUTO_INCREMENT,
              `job_title` VARCHAR(50) NOT NULL,
              `job_salary` INT(11) NOT NULL,
              PRIMARY KEY(job_id));

insert into jobs (`job_title`, `job_salary`) Select 'Büfés', 1200 Where not exists(select * from jobs where job_title='Büfés');
insert into jobs (`job_title`, `job_salary`) Select 'Manager', 1500 Where not exists(select * from jobs where job_title='Manager');
insert into jobs (`job_title`, `job_salary`) Select 'Kasszás', 1300 Where not exists(select * from jobs where job_title='Kasszás');
insert into jobs (`job_title`, `job_salary`) Select 'Jegykezelő', 1300 Where not exists(select * from jobs where job_title='Jegykezelő');

CREATE TABLE IF NOT EXISTS `financy`(
              `Day` DATE NOT NULL,
              `dChange` DOUBLE DEFAULT NULL,
              `dIncome` DOUBLE DEFAULT NULL,
              `dOutcome` DOUBLE DEFAULT NULL,
              `mIncome` DOUBLE DEFAULT NULL,
              `mOutcome` DOUBLE DEFAULT NULL,
              `netIncome` DOUBLE DEFAULT NULL,
              PRIMARY KEY(Day));

CREATE TABLE IF NOT EXISTS `employees` (
              `emp_id` INT(11) AUTO_INCREMENT,
              `name` VARCHAR(50) NOT NULL,
              `phone_number` VARCHAR(12) DEFAULT NULL,
              `birth_date` DATE NOT NULL,
              `gender` VARCHAR(50) NOT NULL,
              `hire_date` DATE NOT NULL,
              `tax_number` BIGINT(20) DEFAULT NULL,
              `taj_number` BIGINT(20) NOT NULL,
              `jobs_Title_ID` INT(11) DEFAULT NULL,
              `mail` VARCHAR(50) DEFAULT NULL,
              `address` VARCHAR(100) DEFAULT NULL,
              PRIMARY KEY(emp_id));

ALTER TABLE kenyioffice.employees 
  ADD CONSTRAINT employees_to_jobs FOREIGN KEY (jobs_Title_ID)
    REFERENCES kenyioffice.jobs(job_id);

CREATE TABLE IF NOT EXISTS `wages` (
              day DATE NOT NULL, 
              Hours_monthly DOUBLE NOT NULL,
              monthly_salary DOUBLE NOT NULL,
              employee_id INT(11) NOT NULL,
              PRIMARY KEY(day));

ALTER TABLE kenyioffice.wages 
  ADD CONSTRAINT wages_to_employee FOREIGN KEY (employee_id)
    REFERENCES kenyioffice.employees(emp_id);

CREATE TABLE IF NOT EXISTS `workinghours` (
              id_wkhrs INT(11) NOT NULL, 
              from_date DATETIME(6) DEFAULT NULL, 
              to_date DATETIME(6) DEFAULT NULL, 
              minutes DOUBLE DEFAULT NULL, 
              online TINYINT(1) NOT NULL DEFAULT 0,
              PRIMARY KEY(id_wkhrs));

ALTER TABLE kenyioffice.workinghours 
  ADD CONSTRAINT wrk_to_employee FOREIGN KEY (id_wkhrs)
    REFERENCES kenyioffice.employees(emp_id) ON DELETE CASCADE;