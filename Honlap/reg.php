﻿<?php 
include 'dbconnect.php';
$conn = MySQLServerConnecter();
$query="insert into honlapusers (`Username`, `Password`, `Fullname`, `Address`, `Phonenumer`) values ('".$_POST['f_felhnev']."','".md5($_POST['f_jelszo'])."','".$_POST['f_nev']."','".$_POST['f_cim']."', '".$_POST['f_telefon']."')";
$conn->query($query);
$conn->close();
echo "<h1>Sikeres regisztráció!</h1>";
?>
<html>
	<form action="index.php" method="post">
	<input type="submit" value="Vissza"/>
	</form> 
</html>