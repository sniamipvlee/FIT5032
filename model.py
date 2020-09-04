import warnings
warnings.simplefilter('ignore')
import pandas as pd
import pymysql
import sklearn
from sklearn.linear_model import LogisticRegression
from sklearn.model_selection import train_test_split
from sklearn.linear_model import LogisticRegressionCV
import joblib
from sqlalchemy import create_engine


host="heartstationdatabase.cvkcfi0mgtaa.us-east-1.rds.amazonaws.com"
port=int(3306)
dbname="heart_data"
user="heartstation"
password="PG5cBGTGgqrMv4Dz1lYu"

engine = create_engine('mysql+pymysql://{user}:{password}@{host}:{port}/{dbname}'.format(user=user, password=password, host=host, port=port, dbname=dbname))

heart_df = pd.read_sql_table('heart_df', engine)

x=heart_df.iloc[:,:-1]
y=heart_df.iloc[:,-1]
x_train, x_test, y_train, y_test = train_test_split(x, y, test_size=.20, random_state=5)

lr = LogisticRegressionCV(cv=5, random_state=0).fit(x_train, y_train)

joblib.dump(lr, 'model.pkl')
print("Model dumped!")

model_columns = list(x.columns)

joblib.dump(model_columns, 'model_columns.pkl')
print("Models columns dumped!")

app = Flask(__name__)
lr = joblib.load('model.pkl')
model_columns = joblib.load('model_columns.pkl')

@app.route('/')
def Main():
    if lr:
        try:
            info = []
            info.append(request.args.get("sex"))
            info.append(request.args.get("age"))
            info.append(request.args.get("currentSmoker"))
            a = np.array(info)
            result = str(round(float(lr.predict_proba(a)[:, 1] * 100), 2)) + '%'
            return result
        except:
            return jsonify({'trace': traceback.format_exc()})
    else:
        return 'No model here to use'