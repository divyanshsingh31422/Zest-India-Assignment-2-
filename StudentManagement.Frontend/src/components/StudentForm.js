import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { studentsAPI } from '../services/api';

const StudentForm = ({ token }) => {
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    age: '',
    course: '',
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [isEditing, setIsEditing] = useState(false);
  
  const navigate = useNavigate();
  const { id } = useParams();

  useEffect(() => {
    if (id) {
      setIsEditing(true);
      fetchStudent();
    }
  }, [id]);

  const fetchStudent = async () => {
    try {
      setLoading(true);
      const student = await studentsAPI.getById(id);
      setFormData({
        name: student.name,
        email: student.email,
        age: student.age.toString(),
        course: student.course,
      });
    } catch (err) {
      setError('Failed to fetch student details. Please try again.');
      console.error('Error fetching student:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    setSuccess('');

    // Validate age
    const age = parseInt(formData.age);
    if (isNaN(age) || age < 18 || age > 100) {
      setError('Age must be between 18 and 100.');
      setLoading(false);
      return;
    }

    try {
      const studentData = {
        name: formData.name.trim(),
        email: formData.email.trim(),
        age: age,
        course: formData.course.trim(),
      };

      if (isEditing) {
        await studentsAPI.update(id, studentData);
        setSuccess('Student updated successfully!');
      } else {
        await studentsAPI.create(studentData);
        setSuccess('Student created successfully!');
      }

      setTimeout(() => {
        navigate('/students');
      }, 1500);
    } catch (err) {
      setError(err.response?.data?.message || `Failed to ${isEditing ? 'update' : 'create'} student. Please try again.`);
      console.error('Error saving student:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleCancel = () => {
    navigate('/students');
  };

  if (loading && isEditing) {
    return (
      <div className="form-container">
        <div style={{ textAlign: 'center', padding: '2rem' }}>
          <div className="loading" style={{ width: '40px', height: '40px', margin: '0 auto' }}></div>
          <p>Loading student details...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="form-container">
      <h2>{isEditing ? 'Edit Student' : 'Add New Student'}</h2>
      
      {error && <div className="alert alert-error">{error}</div>}
      {success && <div className="alert alert-success">{success}</div>}

      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label htmlFor="name">Name *</label>
          <input
            type="text"
            id="name"
            name="name"
            value={formData.name}
            onChange={handleChange}
            required
            placeholder="Enter student name"
            maxLength="100"
          />
        </div>

        <div className="form-group">
          <label htmlFor="email">Email *</label>
          <input
            type="email"
            id="email"
            name="email"
            value={formData.email}
            onChange={handleChange}
            required
            placeholder="Enter student email"
            maxLength="100"
          />
        </div>

        <div className="form-group">
          <label htmlFor="age">Age *</label>
          <input
            type="number"
            id="age"
            name="age"
            value={formData.age}
            onChange={handleChange}
            required
            placeholder="Enter student age (18-100)"
            min="18"
            max="100"
          />
        </div>

        <div className="form-group">
          <label htmlFor="course">Course *</label>
          <select
            id="course"
            name="course"
            value={formData.course}
            onChange={handleChange}
            required
          >
            <option value="">Select a course</option>
            <option value="Computer Science">Computer Science</option>
            <option value="Mathematics">Mathematics</option>
            <option value="Physics">Physics</option>
            <option value="Chemistry">Chemistry</option>
            <option value="Biology">Biology</option>
            <option value="Engineering">Engineering</option>
            <option value="Business">Business</option>
            <option value="Arts">Arts</option>
            <option value="Medicine">Medicine</option>
            <option value="Law">Law</option>
          </select>
        </div>

        <div style={{ display: 'flex', gap: '1rem', justifyContent: 'flex-end' }}>
          <button
            type="button"
            className="btn btn-secondary"
            onClick={handleCancel}
            disabled={loading}
          >
            Cancel
          </button>
          <button
            type="submit"
            className="btn btn-primary"
            disabled={loading}
          >
            {loading ? (
              <>
                <span className="loading"></span> {isEditing ? 'Updating...' : 'Creating...'}
              </>
            ) : (
              isEditing ? 'Update Student' : 'Create Student'
            )}
          </button>
        </div>
      </form>
    </div>
  );
};

export default StudentForm;
