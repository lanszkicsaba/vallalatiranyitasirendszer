<?php
include 'dbconnect.php';
session_start();
?>
<head>
		<meta charset="UTF-8">
		<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
</head>
<body>
<div class="container">
<?php

//ha bevan lépve
if (count($_SESSION) > 0 && $_SESSION["login"] == "TRUE") {
    $conn = MySQLServerConnecter(); //adatbázis kapcsolat létrehozása
    //lekérés a felhasználó adatait
    $query = "SELECT Id, Fullname, Address, Phonenumer, Taxnumber FROM honlapusers WHERE Username='" . $_SESSION['user'] . "'";
    //lekérés végre hajtása
    $s = $conn->query($query);
    $conn->close(); //kapcsolat lezárása

    $row = $s->fetch_assoc(); //sorok felbontása

    $writer = new XMLWriter(); //XML író
    $writer->openUri('rendeles.xml'); //XML neve
    $writer->startDocument('1.0', 'UTF-8'); //A Formátum fejlécs
    $writer->startElement('rendeles');      //levél létrehozása
    $writer->startElement("rendeloadatai"); //Melléklevél létrehozása
    $writer->writeElement('nev', $row["Fullname"]); //név adat
    $writer->writeElement('cim', $row["Address"]);  //cím adat
    $writer->writeElement('telefonszam', $row["Phonenumer"]); //Telefonszám
    $writer->writeElement('adoszam', $row["Taxnumber"]); //adószám
    $writer->endElement(); //Melléklevél lezárása
    

    $conn = MySQLServerConnecter(); //kapcsolat megnyitás
    date_default_timezone_set("Europe/Budapest"); //időzóna
    $queryinsert = "INSERT INTO rendelesek VALUES (NULL,'" . date("Y.m.d h:i:sa") . "','" . $row["Id"] . "');"; //rendelés azonosítók beszúrása
    $conn->query($queryinsert); //query végrehajtása
    $queryget = "SELECT max(id) as id FROM rendelesek;"; //query
    $s = $conn->query($queryget); //query lefutatása
    $conn->close(); //kapcsolat lezárása

    $rendelesid = $s->fetch_assoc(); //rendelési azonosító
	$writer->startElement("rendelesadatai"); //Melléklevél létrehozása
	$writer->writeElement('rendid',date("Y")."/".$rendelesid["id"]); //Rendelés sorszáma
	$writer->writeElement('rendido',date("Y.m.d h:i:sa")); //Rendelés pontos ideje
	$writer->endElement(); //Melléklevél lezárása
    $writer->startElement("termekekadatai"); //melléklevél létrehozása
	
    for ($index = 0; $index < count($_SESSION["cart2"]); $index++) {

        $conn = MySQLServerConnecter(); //kapcsolat megnyitás

        $query = "SELECT id, termeknev, ar FROM termekek WHERE id=" . $_SESSION["cart2"][$index]; //lekéri az i. megrendelni kívánt termék adatait

        $s = $conn->query($query); //query lefutatása
        $conn->close(); //kapcsolat lezárása

        $row = $s->fetch_assoc(); //sorok felbontása
        if (isset($_POST[$index])) {
			$writer->startElement("termek"); //Melléklevél létrehozása
            $writer->writeElement('termekneve', $row["termeknev"]); //termékneve
            $writer->writeElement('mennyiseg', $_POST[$index] . ' db'); //termék mennyisége
            $writer->writeElement('ara', $_POST[$index] * $row["ar"] . ' Ft'); //ára számítva
			$writer->endElement(); //Melléklevél lezárása
            $conn = MySQLServerConnecter(); //kapcsolat megnyitás
            $queryinsert = "INSERT INTO rendeles_adatok VALUES (NULL,'" . $rendelesid["id"] . "','" . $row["id"] . "','" . $_POST[$index] . "');"; //megrendelt termékek feltöltése
            $conn->query($queryinsert); //query lefutatása
            $conn->close(); //kapcsolat lezárása
        }
    }
    $writer->endElement(); //melléklevél lezárás
    $writer->endElement(); //levéllezárás
    $writer->endDocument(); //dokumentum lezárás
    $writer->flush(); //fájl kiírása
    echo '<h2>Köszönjük megrendelését! Hamarosan szállítjuk a megrendelését!</h2>';
    $_SESSION["cart2"] = array(); //lista ürítése
} else {
    //nincs bejelentkezve visszadobja az index.php-re
    echo '<meta http-equiv="refresh" content="0; URL=index.php">';
}
?>
<html>
	<form action="index.php" method="post">
		<input class="btn btn-default"  type="submit" value="Vissza"/>
	</form> 
</html>
</div>
</body>