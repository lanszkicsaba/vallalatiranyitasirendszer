<!DOCTYPE html>
<html>
    <head>
        <meta charset="UTF-8">
        <title>Regisztráció</title>
    </head>
    <body>
        <h1>Regisztráció</h1>
        <script>
            function validateForm() {
                var nev = document.forms["reg"]["f_nev"].value;
				var felh = document.forms["reg"]["f_felhnev"].value;
				var cim = document.forms["reg"]["f_cim"].value;
				var tel = document.forms["reg"]["f_telefon"].value;
				var pw = document.forms["reg"]["f_jelszo"].value;
				var pwk = document.forms["reg"]["f_kontroll"].value;
                if (nev == "" || felh == "" || cim == "" || tel == "" || pw == "" || pwk == "") {
                    alert("Kérem minden mezőt töltsön ki!");
                    return false;
                }
				if (pw != pwk){
					alert("A két jelszó nem egyezik!");
					return false;
				}
            }
        </script>
        <?php
        echo '<form name="reg" action="reg.php" onsubmit="return validateForm()" method="post">
				Név:<br> <input type="text" name="f_nev"><br>
				<br>
				Felhasználónév:<br> <input type="text" name="f_felhnev"><br>
				<br>
				Cím:<br> <input type="text" name="f_cim"><br>
				<br>
				Telefonszám:<br> <input type="number" name="f_telefon"><br>
				<br>
				Jelszó:<br> <input type="password" name="f_jelszo"><br>
				<br>
				Jelszó mégegyszer<br> <input type="password" name="f_kontroll"><br>
				<br>
				<input type="submit" value="Regisztráció" name="submit">
				<input type="reset">
			</form>';
        ?>
    </body>
</html>