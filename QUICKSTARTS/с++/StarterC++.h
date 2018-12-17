#if defined(_MSC_VER) && (_MSC_VER >= 1200)
#pragma once
#endif

#ifndef _MY_STRATEGY_H_
#define _MY_STRATEGY_H_

#include "Strategy.h"
#include "math.h"

const double EPS = 1e-5;

const double BALL_RADIUS = 2.0;
const double ROBOT_MAX_RADIUS = 1.05;
const double MAX_ENTITY_SPEED = 100.0;
const double ROBOT_MAX_GROUND_SPEED = 30.0;
const double ROBOT_MAX_JUMP_SPEED = 15.0;

const double JUMP_TIME = 0.2;
const double MAX_JUMP_HEIGHT = 3.0;

struct Point2D {
    double x {0.0};
    double z {0.0};
    Point2D() {}
    Point2D(double x, double z) : x(x), z(z) {}
    void set(double x_, double z_) { x = x_; z = z_; }
    double dist() { return sqrt(x*x + z*z); }
    Point2D normalize(double len) { return { x/len, z/len }; }
    double distTo(double x_, double z_) { return sqrt((x-x_)*(x-x_) + (z-z_)*(z-z_)); }
    double distTo(Point2D p) { return distTo(p.x, p.z); }
    Point2D operator+(Point2D p) { return { x+p.x, z+p.z }; }
    Point2D operator*(double val) { return { x*val, z*val }; }
    void operator*=(double val) { x *= val; z *= val; }
};

struct Point3D : Point2D {
    double y {0.0}; // высота
    Point3D() {}
    Point3D(double x, double z, double y) : y(y) { Point2D::set(x, z); }
    void set(double x_, double z_, double y_) { x = x_; z = z_; y = y_; }
    double dist() { return sqrt(x*x + y*y + z*z); }
    double distTo(double x_, double z_, double y_) { return sqrt((x-x_)*(x-x_) + (y-y_)*(y-y_) + (z-z_)*(z-z_)); }
    double distTo(Point3D p) { return distTo(p.x, p.z, p.y); }
    Point3D operator+(Point3D p) { return { x+p.x, z+p.z, y+p.y };}
    Point3D operator*(double val) { return { x*val, z*val, y*val }; }
    void operator*=(double val) { x *= val; z *= val; y *= val; }
};

class MyStrategy : public Strategy {
public:

    Point3D ball;
    Point3D ball_v;

    MyStrategy();

    void act(const model::Robot& me, const model::Rules& rules, const model::Game& world, model::Action& action) override;

};

#endif
