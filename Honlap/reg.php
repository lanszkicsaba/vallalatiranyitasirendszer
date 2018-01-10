<!DOCTYPE html>
<html>
	<head>
        <meta charset="UTF-8">
		<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
    </head>
<html>
<div class="container">
	<?php 
	if(isset($_POST['g-recaptcha-response'])&& $_POST['g-recaptcha-response']){
	include 'dbconnect.php';
	$conn = MySQLServerConnecter(); //Adatbázis kapcsolat létrehozása
        $pwd= md5($_POST['f_jelszo']); //Jelszó átadása $pwd-nek
        $s=$conn->prepare("insert into honlapusers (`Username`, `Password`, `Fullname`, `Address`, `Phonenumer`, `Taxnumber`) values (?,?,?,?,?,?)"); //MYSQL statement létrehozása
        $s->bind_param("ssssss", $_POST['f_felhnev'], $pwd, $_POST['f_nev'], $_POST['f_cim'], $_POST['f_telefon'], $_POST['f_ado']); //Változók hozzárendelése a statementhez
        
        $s->execute(); //Statement futtatása
        $result = $s->get_result(); //Lekérdezés eredményének hozzárendelése a $resulthoz
	$conn->close(); //Csatlakozás lezárása
        
	echo "<h1>Sikeres regisztráció!</h1>";
	}
	else{
		header("Location:regisztracio.php");
	}
	?>
	<html>
		<form action="index.php" method="post">
		<input class="btn btn-default" type="submit" value="Vissza"/>
		</form> 
	</html>
</div>