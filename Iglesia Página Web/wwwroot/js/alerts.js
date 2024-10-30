document.getElementById('Recover').addEventListener('submit', function (event) {
    event.preventDefault(); 

    Swal.fire({
        title: '¿Estás seguro?',
        text: "Se enviara un correo con una contraseña temporal para la recuperación de la misma!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sí, enviar!',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            
            event.target.submit();
        }
    });


});

