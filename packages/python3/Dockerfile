FROM python:3.7.1

RUN pip install numpy scipy cython numba
RUN pip install https://download.pytorch.org/whl/cpu/torch-1.0.0-cp37-cp37m-linux_x86_64.whl

COPY . /project
WORKDIR /project
