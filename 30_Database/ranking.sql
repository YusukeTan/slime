-- phpMyAdmin SQL Dump
-- version 5.0.4
-- https://www.phpmyadmin.net/
--
-- ホスト: 127.0.0.1
-- 生成日時: 2022-06-12 09:55:57
-- サーバのバージョン： 10.4.17-MariaDB
-- PHP のバージョン: 7.2.34

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- データベース: `techtest`
--

-- --------------------------------------------------------

--
-- テーブルの構造 `ranking`
--

CREATE TABLE `ranking` (
  `Id` int(11) NOT NULL,
  `Name` varchar(30) NOT NULL,
  `Time` varchar(10) NOT NULL,
  `Date` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- テーブルのデータのダンプ `ranking`
--

INSERT INTO `ranking` (`Id`, `Name`, `Time`, `Date`) VALUES
(2026, 'test', '00:03', '2022-06-11 22:48:20');

--
-- ダンプしたテーブルのインデックス
--

--
-- テーブルのインデックス `ranking`
--
ALTER TABLE `ranking`
  ADD PRIMARY KEY (`Id`);

--
-- ダンプしたテーブルの AUTO_INCREMENT
--

--
-- テーブルの AUTO_INCREMENT `ranking`
--
ALTER TABLE `ranking`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2027;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
