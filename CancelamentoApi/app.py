import requests
from flask_sqlalchemy import SQLAlchemy
from datetime import datetime
from flask import Flask, jsonify
from flask_swagger_ui import get_swaggerui_blueprint
from flask_cors import CORS

app = Flask(__name__)
app.config['SQLALCHEMY_DATABASE_URI'] = "sqlite://"

SWAGGER_URL = '/swagger/'
API_URL = '/static/swagger.json'
swaggerui_blueprint = get_swaggerui_blueprint(SWAGGER_URL, API_URL, config={'app_name': 'API-Cancela-Reserva'})
app.register_blueprint(swaggerui_blueprint)

db = SQLAlchemy(app)

CORS(app, resources={r"/api/*": {"origins": "*"}})


class CancelamentoModel(db.Model):
    __tablename__ = "cancelamento"
    idCancelamento = db.Column(db.Integer, primary_key=True)
    idReserva = db.Column(db.Integer, nullable=False)
    idHotel = db.Column(db.Integer, nullable=False)
    idQuarto = db.Column(db.Integer, nullable=False)
    DataCancelamento = db.Column(db.DateTime, nullable=False)


def toDict(cancelamento):
    return {
        "DataC": cancelamento.DataCancelamento,
        "Id": cancelamento.idCancelamento,
        "Id_hotel": cancelamento.idHotel,
        "Id_quarto": cancelamento.idQuarto,
        "Id_reserva": cancelamento.idReserva
    }


@app.route('/CancelaReserva/<id_reserva>', methods=['GET'])
def consultarCancelamento(id_reserva):
    try:
        data = CancelamentoModel.query.filter_by(idReserva=id_reserva).first()
        if data:
            return data
        return jsonify({'error': 'Cancelamento não encontrado com esse ID'}), 404
    except ValueError as err:
        return jsonify({'error': f"Erro ao consultar o cancelamento"}), 400


@app.route('/CancelaReserva/<id_reserva>', methods=['POST'])
def cancela_reserva(id_reserva):
    try:
        reserva = requests.delete(f'http://127.0.0.1:5001/reservar/{id_reserva}', timeout=0.001)
    except requests.exceptions.ConnectionError:
        return jsonify({'error': f'Erro ao realizar a cancelamento, Falha ao conectar com api de reservas'}), 400

    if reserva.status_code != 200:
        return jsonify({'error': 'Erro ao cancelar, reserva com esse ID não foi encontrada'}), 404

    data = reserva.json()
    if not data:
        return jsonify({'error': 'Erro ao realizar a cancelamento'}), 400

    dataAtual = datetime.now()
    try:
        cancelamento = CancelamentoModel(idReserva=data["idreserva"], idHotel=data["idhotel"],
                                         idQuarto=data["idquarto"], DataCancelamento=dataAtual)
        db.session.add(cancelamento)
        db.session.commit()
        return jsonify(toDict(cancelamento)), 201
    except ValueError:
        return jsonify({'error': f"Erro ao realizar a cancelamento"}), 400


if __name__ == '__main__':
    with app.app_context():
        db.create_all()
        app.run(host='127.0.0.1', port=5003)
