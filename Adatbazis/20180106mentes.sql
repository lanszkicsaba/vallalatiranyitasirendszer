-- phpMyAdmin SQL Dump
-- version 4.0.10deb1
-- http://www.phpmyadmin.net
--
-- Hoszt: localhost
-- Létrehozás ideje: 2018. Jan 06. 01:38
-- Szerver verzió: 5.6.33-0ubuntu0.14.04.1
-- PHP verzió: 5.6.32-1+ubuntu14.04.1+deb.sury.org+2

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Adatbázis: `csaba`
--

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `asztaliusers`
--

CREATE TABLE IF NOT EXISTS `asztaliusers` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `fullname` varchar(255) NOT NULL,
  `name` varchar(255) NOT NULL,
  `passwd` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=2 ;

--
-- A tábla adatainak kiíratása `asztaliusers`
--

INSERT INTO `asztaliusers` (`id`, `fullname`, `name`, `passwd`) VALUES
(1, 'Teszt Elek', 'test', 'test');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `honlapusers`
--

CREATE TABLE IF NOT EXISTS `honlapusers` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Username` varchar(255) CHARACTER SET latin1 NOT NULL,
  `Password` varchar(255) CHARACTER SET latin1 NOT NULL,
  `Fullname` varchar(255) CHARACTER SET latin1 NOT NULL,
  `Address` varchar(255) CHARACTER SET utf8 COLLATE utf8_hungarian_ci NOT NULL,
  `Phonenumer` varchar(255) CHARACTER SET latin1 NOT NULL,
  `Taxnumber` varchar(12) COLLATE latin2_hungarian_ci NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin2 COLLATE=latin2_hungarian_ci AUTO_INCREMENT=3 ;

--
-- A tábla adatainak kiíratása `honlapusers`
--

INSERT INTO `honlapusers` (`Id`, `Username`, `Password`, `Fullname`, `Address`, `Phonenumer`, `Taxnumber`) VALUES
(1, 'test', '098f6bcd4621d373cade4e832627b4f6', 'Test Elek', '3400 Mezőkövesd, Mátyás király út 124.', '06307239293', '15834481210'),
(2, 'test1', '5a105e8b9d40e1329780d62ea2265d8a', 'Rend Elek', '3300 Eger, Malom út 45.', '06301234567', '12345132121');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `rendelesek`
--

CREATE TABLE IF NOT EXISTS `rendelesek` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `rend_ido` datetime NOT NULL,
  `rendelo_id` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=15 ;

--
-- A tábla adatainak kiíratása `rendelesek`
--

INSERT INTO `rendelesek` (`ID`, `rend_ido`, `rendelo_id`) VALUES
(1, '2018-01-03 00:00:00', 1),
(2, '2018-01-03 00:00:00', 1),
(3, '2018-01-03 00:00:00', 1),
(4, '2018-01-03 01:08:31', 1),
(5, '2018-01-03 01:13:57', 1),
(6, '2018-01-03 01:27:48', 1),
(7, '2018-01-03 01:28:29', 1),
(8, '2018-01-06 12:07:35', 1),
(9, '2018-01-06 12:11:35', 1),
(10, '2018-01-06 12:15:28', 1);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `rendeles_adatok`
--

CREATE TABLE IF NOT EXISTS `rendeles_adatok` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `azon` int(11) NOT NULL,
  `termek_id` int(11) NOT NULL,
  `termek_db` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `azon_2` (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci AUTO_INCREMENT=12 ;

--
-- A tábla adatainak kiíratása `rendeles_adatok`
--

INSERT INTO `rendeles_adatok` (`id`, `azon`, `termek_id`, `termek_db`) VALUES
(1, 1, 1, 11),
(2, 2, 1, 14),
(3, 3, 1, 15),
(4, 4, 2, 10),
(5, 5, 1, 4),
(6, 6, 1, 1),
(7, 7, 2, 1),
(8, 8, 1, 4),
(9, 8, 2, 5),
(10, 9, 1, 1),
(11, 10, 1, 1);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `termekek`
--

CREATE TABLE IF NOT EXISTS `termekek` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `termeknev` varchar(255) CHARACTER SET latin2 COLLATE latin2_hungarian_ci NOT NULL,
  `ar` int(11) NOT NULL,
  `mennyiseg` int(11) NOT NULL,
  `kategoria` varchar(255) CHARACTER SET latin2 COLLATE latin2_hungarian_ci NOT NULL,
  `leiras` varchar(255) CHARACTER SET latin2 COLLATE latin2_hungarian_ci NOT NULL,
  `suly` int(11) NOT NULL,
  `kep` varchar(255) CHARACTER SET latin2 COLLATE latin2_hungarian_ci NOT NULL,
  `keszleten` tinyint(1) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=3 ;

--
-- A tábla adatainak kiíratása `termekek`
--

INSERT INTO `termekek` (`id`, `termeknev`, `ar`, `mennyiseg`, `kategoria`, `leiras`, `suly`, `kep`, `keszleten`) VALUES
(1, 'Barack', 190, 200, 'Gyümölcs', 'Finom érett hazai', 411, 'barack.jpg', 0),
(2, 'Elektromos kisautó', 50000, 10, 'Játék elektromos kisautó', 'Akkumulátoros, csak 12 év felett', 600, 'speedy-elektromos-kisauto-6v-r-02875_61fedfc085c6a684570d12ae2a7b4e40.jpg', 1);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
