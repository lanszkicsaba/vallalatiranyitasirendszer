<!DOCTYPE html>
<html>
    <head>
        <meta charset="UTF-8">
		<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
        <title>Regisztráció</title>
        <script src='https://www.google.com/recaptcha/api.js'></script>
    </head>
    <?php
    session_start();
    ?>
    <body>
	<div class="container">
        <div class="page-header">
				<h1>Regisztráció</h1>
			</div>
        <script>
            function validateForm() {
                var nev = document.forms["reg"]["f_nev"].value;
                var felh = document.forms["reg"]["f_felhnev"].value;
                var cim = document.forms["reg"]["f_cim"].value;
                var tel = document.forms["reg"]["f_telefon"].value;
                var pw = document.forms["reg"]["f_jelszo"].value;
                var pwk = document.forms["reg"]["f_kontroll"].value;
                var ado = document.forms["reg"]["f_ado"].value;
                if (nev == "" || felh == "" || cim == "" || tel == "" || ado == "" || pw == "" || pwk == "") {
                    alert("Kérem minden mezőt töltsön ki!");
                    return false;
                }
                if (pw != pwk) {
                    alert("A két jelszó nem egyezik!");
                    return false;
                }
            }
        </script>
        <?php
            echo '<form name="reg" class="form-horizontal" action="reg.php" onsubmit="return validateForm()" method="post">
				<div class="form-group">
					<label class="control-label col-sm-2">Név:</label>
					<div class="col-sm-2">
						<input type="text" class="form-control" name="f_nev" placeholder="Teljes név">
					</div>
				</div>
				<div class="form-group">
					<label class="control-label col-sm-2">Felhasználónév:</label>
					<div class="col-sm-2">
						<input type="text" name="f_felhnev" class="form-control" placeholder="Felhasználónév">
					</div>
				</div>
				<div class="form-group">
					<label class="control-label col-sm-2">Cím:</label>
					<div class="col-sm-2">
						<input type="text" name="f_cim" class="form-control" placeholder="Cím">
					</div>
				</div>
				<div class="form-group">
					<label class="control-label col-sm-2">Telefonszám:</label>
					<div class="col-sm-2">
						<input type="number" name="f_telefon" class="form-control" placeholder="Telefonszám">
					</div>
				</div>
				<div class="form-group">
					<label class="control-label col-sm-2">Adószám:</label>
					<div class="col-sm-2">
						<input type="number" name="f_ado" class="form-control" placeholder="Adószám">
					</div>
				</div>
				<div class="form-group">
					<label class="control-label col-sm-2">Jelszó:</label>
					<div class="col-sm-2">
						<input type="password" name="f_jelszo" class="form-control" placeholder="Jelszó">
					</div>
				</div>
				<div class="form-group">
					<label class="control-label col-sm-2">Jelszó mégegyszer:</label>
					<div class="col-sm-2">
						<input type="password" name="f_kontroll" class="form-control" placeholder="Jelszó"><br>
					</div>
				</div>
				<div class="g-recaptcha" data-sitekey="6LedcDwUAAAAANZsQV5xlt0Vjex6x6n_4cio4-PB"></div><br>
				<input class="btn btn-default" type="submit" value="Regisztráció" name="submit">
				<input class="btn btn-default" type="reset">
			</form>';
			echo '<html>
				<br>
				<form action="index.php" method="post">
				<input class="btn btn-default"  type="submit" value="Vissza"/>
				</form> 
			</html>';
        ?>
	</div>
    </body>
</html>