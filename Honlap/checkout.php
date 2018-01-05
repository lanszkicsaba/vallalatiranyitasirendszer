<?php
include 'dbconnect.php';

session_start();
?>

<?php

if (count($_SESSION) > 0 && $_SESSION["login"] == "TRUE") {
    $conn = MySQLServerConnecter();

    $query = "SELECT Id, Fullname, Address, Phonenumer, Taxnumber FROM honlapusers WHERE Username='" . $_SESSION['user'] . "'";
	
    $s = $conn->query($query);
    $conn->close();

    $row = $s->fetch_assoc();

    $writer = new XMLWriter();
    $writer->openUri('rendeles.xml');
    $writer->startDocument('1.0', 'UTF-8');
    $writer->startElement('rendeles');
    $writer->startElement("rendeloadatai");
    $writer->writeElement('nev', $row["Fullname"]);
    $writer->writeElement('cim', $row["Address"]);
    $writer->writeElement('telefonszam', $row["Phonenumer"]);
    $writer->writeElement('adoszam', $row["Taxnumber"]);
    $writer->endElement();
    $writer->startElement("termekekadatai");
	
	$conn = MySQLServerConnecter();
	date_default_timezone_set("Europe/Budapest");
	$queryinsert = "INSERT INTO rendelesek VALUES (NULL,'".date("Y.m.d h:i:sa")."','".$row["Id"]."');";
	$conn->query($queryinsert);
	$queryget = "SELECT max(id) as id FROM rendelesek;";
	$s = $conn->query($queryget);
	$conn->close();
	
	$rendelesid = $s->fetch_assoc();
    for ($index = 0; $index < count($_SESSION["cart2"]); $index++) {
        $conn = MySQLServerConnecter();

        $query = "SELECT id, termeknev, ar FROM termekek WHERE id=" . $_SESSION["cart2"][$index];
		
        $s = $conn->query($query);
        $conn->close();

        $row = $s->fetch_assoc();
		if (isset($_POST[$index])){
        $writer->writeElement('termekneve', $row["termeknev"]);
        $writer->writeElement('mennyiseg', $_POST[$index] . ' db');
        $writer->writeElement('ara', $_POST[$index] * $row["ar"] . ' Ft');
		
		$conn = MySQLServerConnecter();
		$queryinsert = "INSERT INTO rendeles_adatok VALUES (NULL,'".$rendelesid["id"]."','".$row["id"]."','".$_POST[$index]."');";
		$conn->query($queryinsert);
		$conn->close();
		}
    }
    $writer->endElement();
    $writer->endElement();
    $writer->endDocument();
    $writer->flush();
    echo '<p>Köszönjük megrendelését! Hamarosan szállítjuk a megrendelését!</p>';
    $_SESSION["cart2"]=array();
} else {
    echo '<meta http-equiv="refresh" content="0; URL=index.php">';
}
?>
