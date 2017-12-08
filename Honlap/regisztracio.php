<?php
if($_POST['f_nev'] <> "" and $_POST['f_felhnev'] <> "" and $_POST['f_cim'] <> "" and $_POST['f_telefon'] <> "" and $_POST['f_jelszo'] <> "" and $_POST['f_kontroll'] <> "" ){
	if($_POST['f_jelszo'] == $_POST['f_kontroll']){ 
		include 'dbconnect.php';
		$conn = MySQLServerConnecter();
		$query="select Username from honlapusers where Username='".$_POST['f_felhnev']."';";
		$result=$conn->query($query);
		$row = mysqli_fetch_array($result,MYSQLI_ASSOC);
		$curr_felh = $row["Username"];
		
		if($_POST['f_felhnev'] <> $curr_felh){
			$query="insert into honlapusers (`Username`, `Password`, `Fullname`, `Address`, `Phonenumer`) values ('".$_POST['f_felhnev']."','".md5($_POST['f_jelszo'])."','".$_POST['f_nev']."','".$_POST['f_cim']."', '".$_POST['f_telefon']."')";
			$conn->query($query);
			$conn->close();
	
			echo "<h1>Sikeres regisztráció!</h1>";
		}
		else {
			echo "<h2>A megadott felhasználónév foglalt!</h2>";
		}
	}
	else {
		echo "<h2>A két megadott jelszó nem egyezik!</h2>";
	}
}
else { 
	echo "<h2>Kérem minden mezőt töltson ki!</h2>";
}
	
?>
<html>
	<form action="reg.html" method="post">
	<input type="submit" value="Vissza"/>
	</form> 
</html>