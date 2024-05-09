import mysql.connector
import requests
from datetime import datetime
from flask import Flask, jsonify, request
from flask_swagger_ui import get_swaggerui_blueprint


def conectionDB():
    conection = mysql.connector.connect(
        host='localhost',
        user='root',
        password='',
        database='cancelareserva')
    return conection


app = Flask(__name__)

SWAGGER_URL = '/swagger/'
API_URL = '/static/swagger.json'
swaggerui_blueprint = get_swaggerui_blueprint(SWAGGER_URL, API_URL, config={'app_name': 'API-Cancela-Reserva'})
app.register_blueprint(swaggerui_blueprint)


@app.route('/CancelaReserva/<id_reserva>', methods=['GET'])
def consultarCancelamento(id_reserva):
    conection = conectionDB()
    cursor = conection.cursor(dictionary=True)
    sql = 'SELECT * FROM cancelamento WHERE Id_reserva = %s'
    values = [id_reserva]
    try:
        cursor.execute(sql, values)
        data = cursor.fetchall()
        if data:
            return jsonify(data[0])
        else:
            return jsonify({'error': 'Cancelamento não encontrado com esse ID'}), 404
    except mysql.connector.Error as err:
        print(f"Erro ao consultar o cancelamento: {err}")
        return jsonify({'error': f"Erro ao consultar o cancelamento"}), 400
    finally:
        cursor.close()
        conection.close()


@app.route('/CancelaReserva/<id_reserva>', methods=['POST'])
def cancela_reserva(id_reserva):
    reserva = requests.delete(f'http://127.0.0.1:5001/reservar/{id_reserva}')
    if reserva.status_code != 200:
        return jsonify({'error': 'Erro ao cancelar, reserva com esse ID não foi encontrada'}), 404
    data = reserva.json()
    if not data:
        return jsonify({'error': 'Erro ao realizar a cancelamento'}), 400

    conection = conectionDB()
    cursor = conection.cursor(dictionary=True)
    sql = 'insert into cancelamento (Id_reserva, Id_hotel, Id_quarto, DataC) values (%s, %s, %s, %s)'
    dataAtual = datetime.now()
    print(data["idreserva"])
    print(data["idhotel"])
    print(data["idquarto"])
    print(dataAtual)
    values = [data["idreserva"], data["idhotel"], data["idquarto"], dataAtual]
    try:
        cursor.execute(sql, values)
        conection.commit()
        cancelamento = {
            "DataC": dataAtual,
            "Id": cursor.lastrowid,
            "Id_hotel": data["idhotel"],
            "Id_quarto": data["idquarto"],
            "Id_reserva": data["idreserva"]
        }
        return jsonify(cancelamento), 201

    except mysql.connector.Error as err:
        print(f"Erro ao realizar o cancelamento: {err}")
        return jsonify({'error': f"Erro ao realizar a cancelamento"}), 400
    finally:
        cursor.close()
        conection.close()


if __name__ == '__main__':
    app.run(debug=True, port='5003')
