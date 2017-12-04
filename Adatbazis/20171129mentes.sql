-- phpMyAdmin SQL Dump
-- version 4.7.1
-- https://www.phpmyadmin.net/
--
-- Gép: sql11.freemysqlhosting.net
-- Létrehozás ideje: 2017. Nov 29. 14:52
-- Kiszolgáló verziója: 5.5.53-0ubuntu0.14.04.1
-- PHP verzió: 7.0.22-0ubuntu0.16.04.1

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `sql11207393`
--
CREATE DATABASE IF NOT EXISTS `sql11207393` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci;
USE `sql11207393`;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `asztaliusers`
--

CREATE TABLE `asztaliusers` (
  `id` int(11) NOT NULL,
  `fullname` varchar(255) NOT NULL,
  `name` varchar(255) NOT NULL,
  `passwd` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- A tábla adatainak kiíratása `asztaliusers`
--

INSERT INTO `asztaliusers` (`id`, `fullname`, `name`, `passwd`) VALUES
(1, 'Teszt Elek', 'test', 'test');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `honlapusers`
--

CREATE TABLE `honlapusers` (
  `Id` int(11) NOT NULL,
  `Username` varchar(255) CHARACTER SET latin1 NOT NULL,
  `Password` varchar(255) CHARACTER SET latin1 NOT NULL,
  `Fullname` varchar(255) CHARACTER SET latin1 NOT NULL,
  `Address` varchar(255) CHARACTER SET latin1 NOT NULL,
  `Phonenumer` varchar(255) CHARACTER SET latin1 NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin2 COLLATE=latin2_hungarian_ci;

--
-- A tábla adatainak kiíratása `honlapusers`
--

INSERT INTO `honlapusers` (`Id`, `Username`, `Password`, `Fullname`, `Address`, `Phonenumer`) VALUES
(1, 'test', '098f6bcd4621d373cade4e832627b4f6', 'Test Elek', '3400 Mezőkövesd, Mátyás király út 124.', '06307239293');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `termekek`
--

CREATE TABLE `termekek` (
  `id` int(11) NOT NULL,
  `termeknev` varchar(255) CHARACTER SET latin2 COLLATE latin2_hungarian_ci NOT NULL,
  `ar` int(11) NOT NULL,
  `mennyiseg` int(11) NOT NULL,
  `kategoria` varchar(255) CHARACTER SET latin2 COLLATE latin2_hungarian_ci NOT NULL,
  `leiras` varchar(255) CHARACTER SET latin2 COLLATE latin2_hungarian_ci NOT NULL,
  `suly` int(11) NOT NULL,
  `kep` varchar(255) CHARACTER SET latin2 COLLATE latin2_hungarian_ci NOT NULL,
  `keszleten` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- A tábla adatainak kiíratása `termekek`
--

INSERT INTO `termekek` (`id`, `termeknev`, `ar`, `mennyiseg`, `kategoria`, `leiras`, `suly`, `kep`, `keszleten`) VALUES
(1, 'Barack', 190, 200, 'Gyümölcs', 'Finom érett hazai', 411, 'barack.jpg', 0),
(2, 'Elektromos kisautó', 50000, 5, 'Játék elektromos kisautó', 'Akkumulátoros, csak 12 év felett', 60, 'speedy-elektromos-kisauto-6v-r-02875_61fedfc085c6a684570d12ae2a7b4e40.jpg', 1);

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `asztaliusers`
--
ALTER TABLE `asztaliusers`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `honlapusers`
--
ALTER TABLE `honlapusers`
  ADD PRIMARY KEY (`Id`);

--
-- A tábla indexei `termekek`
--
ALTER TABLE `termekek`
  ADD PRIMARY KEY (`id`);

--
-- A kiírt táblák AUTO_INCREMENT értéke
--

--
-- AUTO_INCREMENT a táblához `asztaliusers`
--
ALTER TABLE `asztaliusers`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;
--
-- AUTO_INCREMENT a táblához `honlapusers`
--
ALTER TABLE `honlapusers`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;
--
-- AUTO_INCREMENT a táblához `termekek`
--
ALTER TABLE `termekek`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
